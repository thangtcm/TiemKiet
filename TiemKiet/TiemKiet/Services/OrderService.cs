using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Claims;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<StatusResponse<Order>> Add(OrderInfoVM orderInfoVM, long staffId)
        {
            StatusResponse<Order> data = new();
            var isValid = await _unitOfWork.OrderRepository.GetAsync(x => x.UserId == orderInfoVM.UserId && (x.Status != OrderStatus.Canncel && x.Status != OrderStatus.Complete));
            if(isValid != null)
            {
                data.IsSuccess = false;
                data.Message = "Người dùng đang có đơn hàng chưa hoàn thành.";
                return data;
            }    
            Order order = new(orderInfoVM, staffId)
            {
                DateCreate = DateTime.UtcNow.ToTimeZone(),
                DateUpdate = DateTime.UtcNow.ToTimeZone(),
            };
            ICollection<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in orderInfoVM.CartItems)
            {
                OrderDetail orderDetail = new()
                {
                    AddIce = item.ProductHasIce,
                    Note = item.ProductNotes,
                    Order = order,
                    Price = item.getPrice(),
                    UpSize = item.ProductUpsize,
                    Quantity = item.ProductQuantity,
                };
                orderDetails.Add(orderDetail);
            }    
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.OrderDetailRepository.AddRange(orderDetails);
            await _unitOfWork.CommitAsync();
            //await _notifyFCMService.SendToGroup("Có đơn hàng mới", $"Bạn vừa nhận được đơn hàng mới từ {order.FullName} (SDT: {order.NumberPhone})");
            data.IsSuccess = true;
            data.Message = "Xác nhận đơn hàng thành công, vui lòng đợi nhân viên xác nhận.";
            return data;
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(x => x.Id == Id);
            if(order == null)
            {
                return false;
            }
            _unitOfWork.OrderRepository.Remove(order);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Order? GetById(int Id)
            => _unitOfWork.OrderRepository.Get(x => x.Id == Id);

        public async Task<Order?> GetByIdAsync(long Id, long? userId = null, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null)
        {
            if(userId.HasValue)
            {
                return await _unitOfWork.OrderRepository.GetAsync(x => x.Id == Id && x.UserId == userId.Value, includes);
            }
            else
            {
                return await _unitOfWork.OrderRepository.GetAsync(x => x.Id == Id, includes);
            }
        }

        public async Task<Order?> GetUserPedingOrder(long? userId = null, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null)
        {
            if (userId.HasValue)
            {
                return await _unitOfWork.OrderRepository.GetAsync(x => x.UserId == userId.Value && (x.Status != OrderStatus.Canncel && x.Status != OrderStatus.Complete), includes);
            }
            else
            {
                return new Order();
            }
        }

        public async Task<ICollection<Order>> GetListAsync(long? userId = null, DateTime date = default, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null)
        {
            date = date == default ? DateTime.UtcNow.ToTimeZone() : date;
            if (userId.HasValue)
            {
                Console.WriteLine($"Date là {date.Date}");
                return await _unitOfWork.OrderRepository.GetAllAsync(x => x.UserId == userId.Value && x.DateCreate.Date == date.Date, includes);
            }
            return await _unitOfWork.OrderRepository.GetAllAsync();
        }

        public async Task<bool> UpdateStatus(long orderId, OrderStatus orderStatus, long staffId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(x => x.Id == orderId);
            if(order != null)
            {
                order.Status = orderStatus;
                order.StaffId = staffId;
                if(orderStatus == OrderStatus.Delivering)
                {
                    order.DatePreparing = DateTime.UtcNow.ToTimeZone();
                }
                order.DateUpdate = DateTime.UtcNow.ToTimeZone();
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Update(OrderInfoVM orderInfoVM, long staffId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(x => x.Id == orderInfoVM.OrderId);
            if(order != null)
            {
                order = new Order(orderInfoVM, staffId);
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Order>> GetPendingDateOrders(long userId, DateTime date, OrderStatus orderStatus, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var policyMessages = new Dictionary<string, int>
                {
                    { Constants.Roles.TiemKietNGT, 1 },
                    { Constants.Roles.TiemKietPNT, 2 },
                    { Constants.Roles.TiemKietNB, 3 }
                };
                foreach (var policy in policyMessages.Keys)
                {
                    Console.WriteLine(policy + "\n\n\n");
                    if (userRoles.Contains(policy))
                    {
                        var branchId = policyMessages[policy];
                        Console.WriteLine(branchId + "\n\n\n");
                        return await _unitOfWork.OrderRepository.GetAllAsync(x => x.BranchId == branchId 
                            && x.DateCreate.Date == date.Date 
                            && x.Status == orderStatus, includes, q => q.OrderByDescending(o => o.Id));
                    }
                }
            }
            return new List<Order>();
        }

        public async Task<bool> HasOrder(long userId)
            => (await _unitOfWork.OrderRepository.GetAsync(x => x.UserId == userId && (x.Status != OrderStatus.Canncel && x.Status != OrderStatus.Complete)) is not null);

        public async Task<ICollection<Order>> GetPendingOrders(int branchId, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? includes = null)
            => await _unitOfWork.OrderRepository.GetAllAsync(x => x.BranchId == branchId && (x.Status != OrderStatus.Canncel && x.Status != OrderStatus.Complete), includes, q => q.OrderByDescending(o => o.Id));
    }
}
