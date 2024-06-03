using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Office.Interop.Excel;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class VoucherService : IVoucherService
    {
        public IUnitOfWork _unitOfWork;
        public VoucherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(Voucher model, long userId)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Code.ToUpper() == model.Code.ToUpper());
            if (voucher == null)
            {
                Voucher create = new(model)
                {
                    IsRemoved = false,
                    UserIdUpdate = userId,
                };
                _unitOfWork.VoucherRepository.Add(create);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == Id);
            if (voucher == null) return false;
            voucher.DateUpdate = DateTime.UtcNow.ToTimeZone();
            voucher.UserIdUpdate = userId;
            voucher.IsRemoved = true;
            _unitOfWork.VoucherRepository.Update(voucher);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Voucher? GetById(int Id)
            => _unitOfWork.VoucherRepository.Get(x => x.Id == Id);

        public async Task<Voucher?> GetByIdAsync(int Id)
            => await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == Id);

        public async Task<Voucher?> GetByIdAsync(int Id, Func<IQueryable<Voucher>, IIncludableQueryable<Voucher, object>> includes)
            => await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<Voucher>> GetListAsync()
            => await _unitOfWork.VoucherRepository.GetAllAsync();

        public async Task<ICollection<Voucher>> GetListAsync(Func<IQueryable<Voucher>, IIncludableQueryable<Voucher, object>> includes)
            => await _unitOfWork.VoucherRepository.GetAllAsync(null, includes);

        public async Task<bool> Update(VoucherInfoVM voucherInfo, long userId)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == voucherInfo.VoucherId);
            if(voucher != null)
            {
                voucher.Code = voucherInfo.VoucherCode;
                voucher.DateUpdate = DateTime.UtcNow.ToTimeZone();
                voucher.DiscountType = voucherInfo.DiscountType;
                voucher.DiscountValue = voucherInfo.DiscountValue;
                voucher.MaxDiscountAmount = voucherInfo.MaxDiscountAmount;
                voucher.MinBillAmount = voucherInfo.MinBillAmount;
                voucher.VoucherName = voucherInfo.VoucherName;
                voucher.UserIdUpdate = userId;
                return true;
            }
            return false;
        }

        public IFormFile ConvertToIFormFile(IFormFile file)
        {
            // Tạo một tên tập tin tạm thời cho tệp .xls
            var tempFilePath = Path.GetTempFileName();

            // Lưu tệp IFormFile vào đĩa
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Mở tệp .xls và chuyển đổi sang .xlsx
            var app = new Microsoft.Office.Interop.Excel.Application();
            var wb = app.Workbooks.Open(tempFilePath);
            var newFilePath = Path.ChangeExtension(tempFilePath, ".xlsx");
            wb.SaveAs(newFilePath, XlFileFormat.xlOpenXMLWorkbook);
            wb.Close();
            app.Quit();

            // Đọc nội dung của tệp .xlsx mới chuyển đổi
            byte[] fileBytes = File.ReadAllBytes(newFilePath);

            // Xóa tệp .xls tạm thời
            File.Delete(tempFilePath);
            File.Delete(newFilePath);

            // Tạo một thể hiện mới của IFormFile từ dữ liệu đã chuyển đổi
            var convertedFile = new FormFile(new MemoryStream(fileBytes), 0, file.Length, file.Name, file.FileName);

            // Trả về tệp IFormFile mới
            return convertedFile;
        }
    }
}
