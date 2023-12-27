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
        ITransactionLogRepository TransactionLogRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        IVoucherUserRepository VoucherUserRepository { get; }
        IManagerVoucherLogRepository ManagerVoucherLogRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IBlogRepository BlogRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IVersionRepository VersionRepository { get; }
        IProductHomeRepository ProductHomeRepository { get; }
        IBannerRepository BannerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
