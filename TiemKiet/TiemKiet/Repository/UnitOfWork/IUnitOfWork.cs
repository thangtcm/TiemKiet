using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICountryRepository CountryRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IBranchRepository BranchRepository { get;  }
        IProductRepository ProductRepository { get; }
        IImageRepository ImageRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
