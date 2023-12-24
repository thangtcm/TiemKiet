using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class DistrictInfoVM
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int ProvinceId { get; set; }

        public DistrictInfoVM()
        {

        }

        public DistrictInfoVM(District model)
        {
            DistrictId = model.Id;
            DistrictName = model.DistrictName;
            ProvinceId = model.ProvinceId;
        }
    }
}
