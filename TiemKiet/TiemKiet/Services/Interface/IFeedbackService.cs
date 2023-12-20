using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IFeedbackService
    {
        public Task<ICollection<Feedback>> GetListAsync();
        public Task<ICollection<Feedback>> GetListAsync(Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>> includes);
        public Task Add(FeedbackInfoVM feedbackInfo, long userId);
        public Task<Feedback?> GetByIdAsync(int Id);
        public Task<Feedback?> GetByIdAsync(int Id, Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>> includes);
        public Feedback? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task Update(FeedbackInfoVM feedbackInfo, long userId);
    }
}
