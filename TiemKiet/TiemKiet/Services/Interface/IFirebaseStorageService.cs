namespace TiemKiet.Services.Interface
{
    public interface IFirebaseStorageService
    {
        public Task<Uri> UploadFile(IFormFile file);
    }
}
