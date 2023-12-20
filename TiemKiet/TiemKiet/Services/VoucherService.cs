using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

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
            Voucher create = new(model)
            {
                IsRemoved = false,
                UserIdUpdate = userId,
            };
            _unitOfWork.VoucherRepository.Add(create);
            await _unitOfWork.CommitAsync();
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
    }
}
