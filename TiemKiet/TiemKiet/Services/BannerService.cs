using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class BannerService : IBannerService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public BannerService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task Add(BannerInfoVM model, long userId)
        {
            if(model.UploadImg != null)
            {
                List<string> imglst = new();
                foreach(var img in model.UploadImg)
                {
                    var upload = await _firebaseStorageService.UploadFile(img);
                    imglst.Add(upload.ToString());
                }
                var bannerall = await _unitOfWork.BannerRepository.GetAllAsync();
                _unitOfWork.BannerRepository.RemoveRange(bannerall);
                ICollection<Banner> lstModel = new List<Banner>();
                foreach(var item in imglst)
                {
                    var banner = new Banner()
                    {
                        DatePublish = DateTime.UtcNow.ToTimeZone(),
                        UrlBanner = item,
                        UserId = userId,
                    };
                    lstModel.Add(banner);
                }
                _unitOfWork.BannerRepository.AddRange(lstModel);
                await _unitOfWork.CommitAsync();
            }    
        }

        public async Task<ICollection<Banner>> GetListAsync()
            => await _unitOfWork.BannerRepository.GetAllAsync();
    }
}
