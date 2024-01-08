using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireStaff)]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogController> _logger;
        private readonly IUserService _userService;
        public BlogController(IBlogService blogService, ILogger<BlogController> logger, IUserService userService)
        {
            _blogService = blogService;
            _logger = logger;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            this.AddToastrMessage("Tải dữ liệu thành công", Enums.ToastrMessageType.Success);
            return View(await _blogService.GetListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogInfoVM blog, IFormFile upload)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập");
                    return View();
                }
                this.AddToastrMessage("Đăng tải bài viết thành công", Enums.ToastrMessageType.Success);
                await _blogService.Add(blog, user.Id, upload);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }    
    }
}
