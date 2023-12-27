using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models.ViewModel;
using TiemKiet.Models;
using TiemKiet.Enums;

namespace TiemKiet.Services.Interface
{
    public interface IOrderService
    {
        public Task<ICollection<Order>> GetPendingOrders(int branchId ,Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null);
        public Task<StatusResponse<Order>> Add(OrderInfoVM orderInfoVM, long staffId);
        public Task<bool> HasOrder(long userId);
        public Task<ICollection<Order>> GetListAsync(long? userId = null, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null);
        public Task<Order?> GetByIdAsync(long Id, long? userId = null, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null);
        public Order? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(OrderInfoVM orderInfoVM, long staffId);
        public Task<bool> UpdateStatus(long orderId, OrderStatus orderStatus, long staffId);
    }
}
