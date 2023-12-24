using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class BannerInfoVM
    {
        public ICollection<string> UrlImageLst { get; set; }
        public ICollection<IFormFile> UploadImg { get; set; }

        public BannerInfoVM() { }
        public BannerInfoVM(List<string> lstImage)
        {
            UrlImageLst = lstImage;
        }
    }
}
