using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.Eventing.Reader;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services
{
    public class VoucherUserService : IVoucherUserService
    {
        public IUnitOfWork _unitOfWork;
        public VoucherUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }    
        public async Task<bool> Add(VoucherUser model)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == model.VoucherId);
            if (voucher == null) return false;
            model.ExpiryDate = DateTime.UtcNow.AddDays(voucher.ExpiryDays).ToTimeZone();
            model.RedeemedDate = DateTime.UtcNow.ToTimeZone();
            _unitOfWork.VoucherUserRepository.Add(model);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var model = await _unitOfWork.VoucherUserRepository.GetAsync(x => x.Id == Id);
            if (model == null) return false;
            _unitOfWork.VoucherUserRepository.Remove(model);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public VoucherUser? GetById(int Id)
            => _unitOfWork.VoucherUserRepository.Get(x => x.Id == Id);

        public async Task<VoucherUser?> GetByIdAsync(int Id)
            => await _unitOfWork.VoucherUserRepository.GetAsync(x => x.Id == Id);

        public async Task<VoucherUser?> GetByIdAsync(int Id, Func<IQueryable<VoucherUser>, IIncludableQueryable<VoucherUser, object>> includes)
            => await _unitOfWork.VoucherUserRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<VoucherUser>> GetListAsync(long userId)
        {
            return await _unitOfWork.VoucherUserRepository.GetAllAsync(x => x.UserIdClaim == userId && x.ExpiryDate > DateTime.UtcNow);
        }

        public async Task<ICollection<VoucherUser>> GetListAsync(long userId, Func<IQueryable<VoucherUser>, IIncludableQueryable<VoucherUser, object>> includes)
        {
            return await _unitOfWork.VoucherUserRepository.GetAllAsync(x => x.UserIdClaim == userId, includes);
        }

        public async Task<ICollection<VoucherUser>> GetListAsync()
            => await _unitOfWork.VoucherUserRepository.GetAllAsync();

        public async Task<StatusResponse<VoucherUser>> ReceiveVoucher(string VoucherCode, long UserId)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Code == VoucherCode);
            StatusResponse<VoucherUser> data = new();
            if (voucher == null || !string.Equals(voucher.Code.ToUpper(), VoucherCode))
            {
                data.IsSuccess = false;
                data.Message = "Mã voucher không hợp lệ.";
                return data;
            }
            var DateTimeNow = DateTime.UtcNow.ToTimeZone();
            var voucherValid = await _unitOfWork.VoucherUserRepository.GetAsync(x => x.VoucherId == voucher.Id && x.ExpiryDate > DateTime.UtcNow && x.UserIdClaim == UserId); 
            if(voucherValid != null)
            {
                data.IsSuccess = false;
                data.Message = "Bạn đã sở hữu voucher này.";
                return data;
            }
            VoucherUser model = new()
            {
                UserIdClaim = UserId,
                VoucherId = voucher.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(voucher.ExpiryDays).ToTimeZone(),
                RedeemedDate = DateTime.UtcNow.ToTimeZone()
            };
            _unitOfWork.VoucherUserRepository.Add(model);
            await _unitOfWork.CommitAsync();
            data.IsSuccess = true;
            data.Message = $"Bạn đã nhận được voucher {voucher.VoucherName} với thời hạn {voucher.ExpiryDays} ngày.";
            return data;
        }    
    }
}
