using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
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
        public BranchInfoVM() { }
        public BranchInfoVM(Branch model)
        {
            BranchId = model.Id;
            UrlGoogleMap = model.UrlGoogleMap;
            BranchName = model.BranchName;
            Imagelst = model.Imagelist is null ? new List<string>() : model.Imagelist.Select(x => x.ImageUrl!).ToList();
            ProvinceName = "";
            DistrictId = model.DistrictId;
        }
    }
}
