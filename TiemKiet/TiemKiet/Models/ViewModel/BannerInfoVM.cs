using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class BannerInfoVM
    {
        public int BannerId { get; set; }
        public string UrlImagePC { get; set; }
        public string UrlImageMobile { get; set; }
        public IFormFile UploadImgMobile { get; set; }
        public IFormFile UploadImg { get; set; }

        public BannerInfoVM() { }
        public BannerInfoVM(Banner model)
        {
            this.BannerId = model.Id;
            this.UrlImageMobile = model.UrlBannerMobile;
            this.UrlImagePC = model.UrlBannerPC;
        }
    }
}
