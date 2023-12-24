using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class VersionInfoVM
    {
        public string? VersionName { get; set; }
        public string? UrlAndroidVersion { get; set; }
        public string? UrlIOSVersion { get; set; }
        public bool IsMaintenance { get; set; }
        public bool IsDeploy { get; set; }
        public VersionInfoVM() { }
        public VersionInfoVM(VersionModel model)
        {
            VersionName = model.VersionName;
            UrlAndroidVersion = model.UrlAndroidVersion;
            UrlIOSVersion = model.UrlIOSVersion;
            IsMaintenance = model.IsMaintenance;
            IsDeploy = model.IsDeploy;
        }
    }
}
