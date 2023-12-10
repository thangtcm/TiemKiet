using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class DistrictInfoVM
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set;}
        public int ProvinceId { get; set; }

        public DistrictInfoVM()
        {

        }

        public DistrictInfoVM(District model)
        {
            this.DistrictId = model.Id;
            this.DistrictName = model.DistrictName;
            this.ProvinceId= model.ProvinceId;
        }
    }
}
