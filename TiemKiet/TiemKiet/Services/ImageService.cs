using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class ImageService : IImageService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _storage;
        public ImageService(IUnitOfWork unitOfWork, IFirebaseStorageService storage)
        {
            _unitOfWork = unitOfWork;
            _storage = storage;
        }

        public async Task<ImageModel> Add(IFormFile upload, long userId)
        {
            var result = await _storage.UploadFile(upload);
            ImageModel model = new ();
            if (result != null)
            {
                model = new()
                {
                    ImageUrl = result.ToString(),
                    PublishUpload = DateTime.Now,
                    UserIdUpload = userId
                };
                _unitOfWork.ImageRepository.Add(model);
                await _unitOfWork.CommitAsync();
            }
            return model;
        }

        public async Task<ICollection<ImageModel>> AddRange(List<IFormFile> uploadLst, long userId)
        {
            ICollection<ImageModel> imageLst = new List<ImageModel>();
            foreach(var upload in uploadLst)
            {
                var result = await _storage.UploadFile(upload);
                if(result != null)
                {
                    imageLst.Add(new ImageModel()
                    {
                        ImageUrl = result.ToString(),
                        PublishUpload = DateTime.Now,
                        UserIdUpload = userId
                    });       
                }
            }
            _unitOfWork.ImageRepository.AddRange(imageLst);
            await _unitOfWork.CommitAsync();
            return imageLst;
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var model = _unitOfWork.ImageRepository.Get(x => x.Id == Id);
            if(model == null)
            {
                return false;
            }
            _unitOfWork.ImageRepository.Remove(model);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public ImageModel? GetById(int Id)
            => _unitOfWork.ImageRepository.Get(x => x.Id == Id);

        public async Task<ImageModel?> GetByIdAsync(int Id)
            => await _unitOfWork.ImageRepository.GetAsync(x => x.Id == Id);

        public async Task<ICollection<ImageModel>> GetListAsync()
            => await _unitOfWork.ImageRepository.GetAllAsync();
    }
}
