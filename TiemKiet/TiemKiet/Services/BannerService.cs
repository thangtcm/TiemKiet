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

        public async Task AddRange(List<BannerInfoVM> model, long userId)
        {
            ICollection<Banner> lstModel = new List<Banner>();
            foreach (var item in model)
            {
                var upload = await _firebaseStorageService.UploadFile(item.UploadImg);
                var uploadmb = await _firebaseStorageService.UploadFile(item.UploadImgMobile);
                var banner = new Banner()
                {
                    DatePublish = DateTime.UtcNow.ToTimeZone(),
                    UrlBannerPC = upload.ToString(),
                    UrlBannerMobile = uploadmb.ToString(),
                    UserId = userId,
                };
                lstModel.Add(banner);
            }

            _unitOfWork.BannerRepository.AddRange(lstModel);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<Banner>> GetListAsync()
            => await _unitOfWork.BannerRepository.GetAllAsync();
    }
}
