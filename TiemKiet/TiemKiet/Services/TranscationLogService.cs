using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class TranscationLogService : ITranscationLogService
    {
        public IUnitOfWork _unitOfWork;
        public TranscationLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(TransactionLogVM transcationInfoVM, long userCustomerId, long userStaffId)
        {
            TransactionLog model = new()
            {
                DateTimePayment = DateTime.UtcNow.ToTimeZone(),
                DiscountPrice = transcationInfoVM.DiscountPrice,
                PointNew = transcationInfoVM.PointNew,
                PointOld = transcationInfoVM.PointOld,
                TotalPrice = transcationInfoVM.TotalPrice,
                ScroreNew = transcationInfoVM.ScroreNew,
                ScroreOld = transcationInfoVM.ScroreOld,
                UserIdCustomer = userCustomerId,
                UserIdStaff = userStaffId
            };
            _unitOfWork.TransactionLogRepository.Add(model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var model = await _unitOfWork.TransactionLogRepository.GetAsync(x => x.Id == Id);
            if (model == null) return false;
            _unitOfWork.TransactionLogRepository.Remove(model);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public TransactionLog? GetById(int Id)
            => _unitOfWork.TransactionLogRepository.Get(x => x.Id == Id);

        public async Task<TransactionLog?> GetByIdAsync(int Id)
            => await _unitOfWork.TransactionLogRepository.GetAsync(x => x.Id == Id);

        public async Task<TransactionLog?> GetByIdAsync(int Id, Func<IQueryable<TransactionLog>, IIncludableQueryable<TransactionLog, object>> includes)
            => await _unitOfWork.TransactionLogRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<TransactionLog>> GetListAsync(long userId, DateTime datenow)
            => await _unitOfWork.TransactionLogRepository.GetAllAsync(x => x.UserIdCustomer == userId && x.DateTimePayment.Date == datenow.Date, null, x => x.OrderByDescending(x => x.Id));

        public async Task<ICollection<TransactionLog>> GetListAsync(long userId, Func<IQueryable<TransactionLog>, IIncludableQueryable<TransactionLog, object>> includes)
            => await _unitOfWork.TransactionLogRepository.GetAllAsync(x => x.UserIdCustomer == userId, includes);

        public async Task<ICollection<TransactionLog>> GetListAsync()
            => await _unitOfWork.TransactionLogRepository.GetAllAsync();
    }
}
