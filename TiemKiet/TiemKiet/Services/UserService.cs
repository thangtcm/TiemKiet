using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Net.WebSockets;
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
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranscationLogService _transcationLogService;
        private readonly HttpClient _client;
        private readonly string goongApiKey = "6EuUBdEOjBJijWIabRTLzxVvhYDNi9cKWGY9UxjI";

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
            ITranscationLogService transcationLogService, RoleManager<ApplicationRole> roleManager, HttpClient client)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _transcationLogService = transcationLogService;
            _roleManager = roleManager;
            _client = client;
        }

        public async Task<ApplicationUser?> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

            Console.WriteLine($"User: {_httpContextAccessor.HttpContext!.User}");

            return user;
        }

        public async Task<ICollection<ApplicationUser>> GetUsers()
            => await _unitOfWork.UserRepository.GetAllAsync();


        public async Task<ResponseListVM<UserInfoVM>> GetUsersWithRoles(int page = 1)
        {

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            int pagesize = 10;
            int totalUsers = users.Count;
            int maxpage = (totalUsers / pagesize) + (totalUsers % 10 == 0 ? 0 : 1);
            int pagenumber = page < 0 ? 1 : page;
            PagedList<ApplicationUser> lst = new(users, pagenumber, pagesize);
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in lst)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            var data = new ResponseListVM<UserInfoVM>()
            {
                Data = userWithRoles.ToList(),
                MaxPage = maxpage
            };
            return data;
        }
        public async Task<ICollection<UserInfoVM>> GetUsersWithRoles()
        {

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in users)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            return userWithRoles.ToList();
        }
        public async Task<ApplicationUser?> GetUser(long userId)
            => await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId && x.IsAction == true);

        public async Task<bool> UpdateUser(UserInfoVM user)
        {
            if (user.UserId < 1000000000) return false;
            var userModel = await _unitOfWork.UserRepository.GetAsync(x => x.Id == user.UserId!);
            if (userModel is null) return false;
            userModel.FullName = user.FullName;
            userModel.Gender = user.Gender ?? Gender.Another;
            DateTime datenow = DateTime.Now;
            if (!String.IsNullOrEmpty(user.Birthday))
            {
                datenow = CallBack.ConvertStringToDateTime(user.Birthday);
            }
            userModel.Birthday = datenow;
            _unitOfWork.UserRepository.Update(userModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<StatusResponse<bool>> RemoveUser(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);
            StatusResponse<bool> data = new();
            if (user != null)
            {
                user.IsAction = false;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
                data.IsSuccess = true;
                data.Message = "Xóa tài khoản thành công";
                return data;
            }
            data.IsSuccess = false;
            data.Message = "Người dùng không hợp lệ.";
            return data;
        }

        public async Task<StatusResponse<CaculateVoucherInfo>> CaculatePrice(long userId, double TotalPrice, double ShipPrice, List<int> VoucherList)
        {
            Console.WriteLine($"userId{userId}");
            var user = await GetUser(userId);
            StatusResponse<CaculateVoucherInfo> data = new();
            if (user == null)
            {
                data.IsSuccess = false;
                data.Message = "{ERROR} Người dùng không hợp lệ.";
                return data;
            }
            double discount = 0, shipdiscount = 0;
            if (VoucherList.Any(item => item == 0))
            {
                discount = CallBack.GetDiscount(user.Score);
            }
            Console.WriteLine($"Giam gia{discount}");
            Console.WriteLine($"TotalPrice{TotalPrice}");
            List<double> DiscountTotal = new();
            var vouchers = await _unitOfWork.VoucherRepository.GetAllAsync(x => VoucherList.Contains(x.Id));
            if (vouchers.Count(x => x.VoucherType == VoucherType.VoucherShip) > 1 || vouchers.Count(x => x.VoucherType == VoucherType.VoucherProduct) > 1)
            {
                data.IsSuccess = false;
                data.Message = "{ERROR} Mỗi loại voucher chỉ được phép chọn 1 voucher.";
                return data;
            }
            foreach (var item in vouchers)
            {
                if (item.MinBillAmount > TotalPrice)
                {
                    data.IsSuccess = false;
                    data.Message = $"Voucher bạn vừa chọn cần hoá đơn tối thiểu {item.MinBillAmount.ToString("C0", System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"))}.";
                    return data;
                }
                switch (item.VoucherType)
                {
                    case VoucherType.VoucherShip:
                        {
                            if (item.DiscountType == DiscountType.Percentage)
                            {
                                shipdiscount = ((ShipPrice * item.DiscountValue / 100) > item.MaxDiscountAmount ? item.MaxDiscountAmount : (ShipPrice * item.DiscountValue / 100));
                            }
                            else
                            {
                                shipdiscount = item.DiscountValue;
                            }
                            break;
                        }
                    case VoucherType.VoucherProduct:
                        {
                            if (item.DiscountType == DiscountType.Percentage)
                            {
                                DiscountTotal.Add(((TotalPrice * item.DiscountValue / 100) > item.MaxDiscountAmount ? item.MaxDiscountAmount : (TotalPrice * item.DiscountValue / 100)));
                            }
                            else
                            {
                                DiscountTotal.Add(item.DiscountValue);
                            }
                            break;
                        }
                }

            }
            CaculateVoucherInfo model = new()
            {
                DiscountPrice = (TotalPrice * discount / 100) + DiscountTotal.Sum(),
                CurrentPrice = TotalPrice,
                UserId = userId,
                VoucherList = VoucherList,
                ShipPrice = (ShipPrice - shipdiscount) < 0 ? 0 : (ShipPrice - shipdiscount),
                DiscountShipPrice = (shipdiscount > ShipPrice) ? ShipPrice : shipdiscount
            };
            model.TotalPrice = TotalPrice - model.DiscountPrice;
            data.IsSuccess = true;
            data.Result = model;
            data.Message = "Tính toán thành công.";
            return data;
        }

        public async Task<ApplicationUser?> GetUserwithPhone([Phone] string Phone)
            => await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == Phone);

        public async Task UpdatePoint(CaculateVoucherInfo model, long userId)
        {
            var user = await GetUser(model.UserId);
            if (user != null)
            {
                double RecivePoint = model.TotalPrice / 1000.0;
                TransactionLogVM _logPayment = new()
                {
                    PointOld = user.Point,
                    TotalPrice = model.TotalPrice,
                    ScroreOld = user.Score,
                    ScroreNew = user.Score + RecivePoint,
                    PointNew = user.Score + RecivePoint,
                    DiscountPrice = model.DiscountPrice
                };
                user.Point += RecivePoint;
                user.Score += RecivePoint;
                await _transcationLogService.Add(_logPayment, model.UserId, userId);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SetUserLocation(double latitude, double longitude)
        {
            //byte[]? oldLatitudeBytes = _httpContextAccessor.HttpContext!.Session.Get(Constants.UserLatitude);
            //byte[]? oldLongitudeBytes = _httpContextAccessor.HttpContext.Session.Get(Constants.UserLongitude);
            var locationUser = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<LocationUser>(Constants.LocationUser) ?? new LocationUser();
            double oldLatitude = 0;
            double oldLongitude = 0;
            if (locationUser != null)
            {
                oldLatitude = locationUser.Latitude;
                oldLongitude = locationUser.Longitude;
            }
            var distance = CalculateDistance(oldLatitude, oldLongitude, latitude, longitude);
            Console.WriteLine($"Khoảng cách là : {distance}");
            if (distance >= 15)
            {
                var formattedAddress = await GetAddressName(latitude, longitude);
                var getDistance = await GetDistance(latitude, longitude);
                if (!string.IsNullOrEmpty(formattedAddress) && getDistance != null)
                {
                    LocationUser model = new()
                    {
                        AddreasUserName = formattedAddress,
                        DistanceBranches = getDistance,
                        Latitude = latitude,
                        Longitude = longitude
                    };
                    _httpContextAccessor.HttpContext.Session.SetObjectAsJson(Constants.LocationUser, model);
                }
            }
        }

        private async Task<List<DistanceBranch>> GetDistance(double latitude, double longitude)
        {
            List<DistanceBranch> distanceBranches = new List<DistanceBranch>();
            var locationBranches = new LocationBranch().Default();
            string geocodingApiUrl = $"https://rsapi.goong.io/DistanceMatrix?origins={latitude},{longitude}&destinations=";

            foreach (var item in locationBranches)
            {
                geocodingApiUrl += $"{item.Latitude},{item.Longitude}|";
            }

            // Loại bỏ dấu "|" cuối cùng nếu có
            geocodingApiUrl = geocodingApiUrl.TrimEnd('|');

            geocodingApiUrl += $"&api_key={goongApiKey}";
            var response = await _client.GetAsync(geocodingApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonObject = JObject.Parse(jsonString);
                if (jsonObject["rows"] is JArray rows && rows.Count > 0 &&
                    rows[0]["elements"] is JArray elements && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        var element = elements[i];
                        var distanceBranch = new DistanceBranch
                        {
                            LocationBranch = locationBranches[i],
                            Distance = element["distance"]?["text"]?.ToString() ?? "N/A",
                            Duration = element["duration"]?["text"]?.ToString() ?? "N/A"
                        };
                        Console.WriteLine($"{locationBranches[i].BranchName} có khoảng cách {distanceBranch.Distance} với thời gian là : {distanceBranch.Duration}");
                        distanceBranches.Add(distanceBranch);
                    }
                }
            }
            return distanceBranches;
        }

        private async Task<string> GetAddressName(double latitude, double longitude)
        {
            string geocodingApiUrl = $"https://rsapi.goong.io/Geocode?latlng={latitude},{longitude}&api_key={goongApiKey}";
            var response = await _client.GetAsync(geocodingApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonObject = JObject.Parse(jsonString);
                if (jsonObject["results"] is JArray results && results.Count > 0)
                {
                    var firstResult = results[0];
                    if (firstResult != null)
                    {
                        var formattedAddressToken = firstResult["formatted_address"];
                        if (formattedAddressToken != null)
                        {
                            var formattedAddress = formattedAddressToken.ToString();
                            Console.WriteLine("Result Address: " + formattedAddress);
                            return formattedAddress;
                        }
                    }
                }
            }
            return "";
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Bán kính trái đất trong km

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // Khoảng cách giữa hai điểm

            return distance * 1000; // Chuyển đổi khoảng cách thành mét
        }

        private double ToRadians(double degree)
        {
            return degree * (Math.PI / 180);
        }

        public async Task<ICollection<ApplicationUser>> GetUsersRange(List<long> ListUserId)
            => await _unitOfWork.UserRepository.GetAllAsync(x => ListUserId.Contains(x.Id));
    }
}
