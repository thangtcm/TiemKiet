using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class ProvinceBranchVM
    {
        public string ProvinceName { get; set; }
        public int ProvinceId { get; set; }
        public int BranchCount { get; set; }
        public ICollection<BranchInfoVM>? branches { get; set; }
        public ICollection<DistrictInfoVM>? districts { get; set; }
    }
}
