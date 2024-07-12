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
using X.PagedList;

namespace TiemKiet.Services
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotifyFCMService _notificationManager;
        private readonly IUserService _userService;
        public OrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, INotifyFCMService notificationManager,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _notificationManager = notificationManager;
            _userService = userService;
        }
        public async Task<StatusResponse<long>> Add(OrderInfoVM orderInfoVM, long staffId)
        {
            StatusResponse<long> data = new();
            var user = await _userService.GetUser(orderInfoVM.UserId);
            if (user == null)
            {
                data.IsSuccess = false;
                data.Message = "Người dùng không tồn tại.";
                return data;
            }
            var isValid = await _unitOfWork.OrderRepository.GetAsync(x => x.UserId == orderInfoVM.UserId && (x.Status != OrderStatus.Canncel && x.Status != OrderStatus.Complete));
            if (isValid != null)
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
                    Price = item.getProductPrice(),
                    UpSize = item.ProductUpsize,
                    Quantity = item.ProductQuantity,
                    ProductId = item.ProductId
                };
                orderDetails.Add(orderDetail);
            }
            order.Status = OrderStatus.WaitingConfirm;
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.OrderDetailRepository.AddRange(orderDetails);
            await _unitOfWork.CommitAsync();
            await _notificationManager.SendToGroup("Có đơn hàng mới", $"Bạn vừa nhận được đơn hàng mới từ {order.FullName} (SDT: {order.NumberPhone})");
            data.IsSuccess = true;
            data.Message = "Xác nhận đơn hàng thành công, vui lòng đợi nhân viên xác nhận.";
            data.Result = order.Id;
            return data;
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(x => x.Id == Id);
            if (order == null)
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
            if (userId.HasValue)
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
            if (order != null)
            {
                var message = string.Empty;
                order.Status = orderStatus;
                order.StaffId = staffId;
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == order.UserId);
                if (user == null) return false;
                double RecivePoint = order.GrandTotal / 1000.0;
                user.Point += RecivePoint;
                user.Score += RecivePoint;
                var calculation = new CaculateVoucherInfo() { UserId = user.Id, CurrentPrice = order.GrandTotal, DiscountPrice = order.Discount, DiscountShipPrice = order.DiscountShip, TotalPrice = order.GrandTotal - order.Discount, ShipPrice = order.ShipTotal, VoucherList = order.ListVoucher.Split(", ").Select(int.Parse).ToList() };
                await _userService.UpdatePoint(
                     calculation,
                     order.UserId ?? 0
                 );
                _unitOfWork.UserRepository.Update(user);
                switch (orderStatus)
                {
                    case OrderStatus.Canncel:
                        {
                            message = $"Đơn hàng (ID: {order.Id}) của bạn đã bị hủy.";
                            break;
                        }
                    case OrderStatus.WaitingConfirm:
                        {
                            message = $"Đơn hàng (ID: {order.Id}) của bạn đã được chuyển sang trạng thái chờ.";
                            break;
                        }
                    case OrderStatus.Preparing:
                        {
                            message = $"Đơn hàng (ID: {order.Id}) của bạn đã được duyệt và đang được chuẩn bị nước.";
                            break;
                        }
                    case OrderStatus.Delivering:
                        {
                            message = $"Đơn hàng (ID: {order.Id}) của bạn đang được giao.";
                            order.DatePreparing = DateTime.UtcNow.ToTimeZone();
                            break;
                        }
                    default:
                        {
                            message = $"Đơn hàng (ID: {order.Id}) của bạn được giao hàng thành công và nhận được {RecivePoint} điểm. Tiệm Kiết cảm ơn và chúc bạn ngon miệng mlem mlem.";
                            break;
                        }
                }
                order.DateUpdate = DateTime.UtcNow.ToTimeZone();
                _unitOfWork.OrderRepository.Update(order);
                await _notificationManager.SendToUser("Thông báo từ đơn hàng", message, user.Id);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Update(OrderInfoVM orderInfoVM, long staffId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(x => x.Id == orderInfoVM.OrderId);
            if (order != null)
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
                List<OrderStatus> listOrderStatus = new() {
                    orderStatus
                };
                if (orderStatus == OrderStatus.Preparing || OrderStatus.Delivering == orderStatus)
                {
                    listOrderStatus.Add(OrderStatus.Preparing);
                    listOrderStatus.Add(OrderStatus.Delivering);
                }
                foreach (var policy in policyMessages.Keys)
                {
                    if (userRoles.Contains(policy))
                    {
                        var branchId = policyMessages[policy];
                        return await _unitOfWork.OrderRepository.GetAllAsync(x => x.BranchId == branchId
                            && x.DateCreate.Date == date.Date
                            && listOrderStatus.Contains(x.Status), includes, q => q.OrderByDescending(o => o.Id));
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
