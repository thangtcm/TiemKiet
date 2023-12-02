using Microsoft.AspNetCore.Mvc;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using X.PagedList;

namespace TiemKiet.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogController> _logger;
        public BlogController(IBlogService blogService, ILogger<BlogController> logger)
        {
            _blogService = blogService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var bloglst = await _blogService.GetListAsync();
            int pagesize = 6;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<BlogPost> lst = new(bloglst, pagenumber, pagesize);
            return View(lst);
        }

        public async Task<IActionResult> Blog(int Id)
        {
            try
            {
                var blog = await _blogService.GetByIdAsync(Id);
                if(blog == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(blog);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }
    }
}
