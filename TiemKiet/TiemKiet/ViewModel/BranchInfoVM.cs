using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class BranchInfoVM
    {
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? UrlGoogleMap { get; set; }
        public ICollection<string>? Imagelst { get; set; }
        public List<IFormFile>? uploadLst { get; set; }
        public string? ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public BranchInfoVM() {  }
        public BranchInfoVM(Branch model)
        {
            this.BranchId = model.Id;
            this.UrlGoogleMap = model.UrlGoogleMap;
            this.BranchName = model.BranchName;
            this.Imagelst = model.Imagelist is null ? new List<string>() :  model.Imagelist.Select(x => x.ImageUrl!).ToList();
            this.ProvinceName = "";
            this.DistrictId = model.DistrictId;
        }
    }
}
