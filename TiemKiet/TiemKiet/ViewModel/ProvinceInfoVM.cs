using TiemKiet.Models;

namespace TiemKiet.ViewModel
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
            this.ProvinceId = model.Id;
            this.ProvinceName = model.CityName;
            this.ShortProvinceName = model.CityNameShort;
            this.CountryId = model.CountryId;
            this.BranchCount = 0;
        }    
    }
}
