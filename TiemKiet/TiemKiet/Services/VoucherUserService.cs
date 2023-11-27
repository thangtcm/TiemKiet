using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.Eventing.Reader;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class VoucherUserService : IVoucherUserService
    {
        public IUnitOfWork _unitOfWork;
        public VoucherUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }    
        public async Task<bool> Add(VoucherUserInfoVM voucherInfoVM)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetAsync(x => x.Id == voucherInfoVM.VoucherId);
            if (voucher == null) return false;
            VoucherUser model = new()
            {
                UserIdClaim = voucherInfoVM.UserIdClaim,
                VoucherId = voucherInfoVM.VoucherId,
                ExpiryDate = DateTime.UtcNow.AddDays(voucher.ExpiryDays),
                RedeemedDate = DateTime.UtcNow,
            };
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

        public async Task<ICollection<VoucherUser>> GetListAsync(long? userId)
        {
            if(userId.HasValue)
            {
                return await _unitOfWork.VoucherUserRepository.GetAllAsync(x => x.UserIdClaim == userId.Value);
            }
            else
            {
                return await _unitOfWork.VoucherUserRepository.GetAllAsync();
            }
        }

        public async Task<ICollection<VoucherUser>> GetListAsync(long? userId, Func<IQueryable<VoucherUser>, IIncludableQueryable<VoucherUser, object>> includes)
        {
            if(userId.HasValue)
            {
                return await _unitOfWork.VoucherUserRepository.GetAllAsync(x => x.UserIdClaim == userId.Value, includes);
            }    
            else
            {
                return await _unitOfWork.VoucherUserRepository.GetAllAsync(null, includes);
            }    
        }
    }
}
