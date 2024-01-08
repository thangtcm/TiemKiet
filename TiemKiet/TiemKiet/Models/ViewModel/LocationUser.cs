namespace TiemKiet.Models.ViewModel
{
    public class LocationUser
    {
        public string AddreasUserName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<DistanceBranch> DistanceBranches { get; set; }
    }

    public class DistanceBranch
    {
        public LocationBranch LocationBranch { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
    }

    public class LocationBranch
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public LocationBranch() { }
        public List<LocationBranch> Default()
        {
            ICollection<LocationBranch> branches = new List<LocationBranch>()
            {
                new()
                {
                    Latitude = 10.803195179191322, 
                    Longitude = 106.71508941012135,
                    BranchId = 1,
                    BranchName = "Tiệm Kiết - Nguyễn Gia Trí"
                },
                new()
                {
                    Latitude = 10.784578843389836,
                    Longitude = 106.69417953895669,
                    BranchId = 2,
                    BranchName = "Tiệm Kiết - Phạm Ngọc Thạch"
                },
                new()
                {
                    Latitude = 10.8041994918,
                    Longitude = 106.64206833285816,
                    BranchId= 3,
                    BranchName = "Tiệm Kiết - Ngô Bệ"
                },
            };

            return branches.ToList();
        }
    }

}
