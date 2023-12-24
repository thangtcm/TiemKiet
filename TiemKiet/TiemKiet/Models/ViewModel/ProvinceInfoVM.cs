using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class ProvinceInfoVM
    {
        public int? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public string? ShortProvinceName { get; set; }
        public int CountryId { get; set; }
        public int BranchCount { get; set; }
        public ProvinceInfoVM() { }
        public ProvinceInfoVM(Province model)
        {
            ProvinceId = model.Id;
            ProvinceName = model.CityName;
            ShortProvinceName = model.CityNameShort;
            CountryId = model.CountryId;
            BranchCount = 0;
        }
    }
}
