using TiemKiet.Data;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private ICountryRepository _countryRepository;
        private IProvinceRepository _provinceRepository;
        private IDistrictRepository _districtRepository;
        private IBranchRepository _branchRepository;
        private IProductRepository _productRepository;
        private IImageRepository _imageRepository;
        private ITransactionLogRepository _transactionLog;
        private IVoucherRepository _voucherRepository;
        private IVoucherUserRepository _voucherUserRepository;
        private IManagerVoucherLogRepository _managerVoucher;
        private IUserTokenRepository _userTokenRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new RoleRepository(_context);
            }
        }
        public ICountryRepository CountryRepository
        {
            get
            {
                return _countryRepository ??= new CountryRepository(_context);
            }
        }

        public IProvinceRepository ProvinceRepository
        {
            get
            {
                return _provinceRepository ??= new ProvinceRepository(_context);
            }
        }

        public IDistrictRepository DistrictRepository
        {
            get
            {
                return _districtRepository ??= new DistrictRepository(_context);
            }
        }
        public IBranchRepository BranchRepository
        {
            get
            {
                return _branchRepository ??= new BranchRepository(_context);
            }
        }
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }
        public IImageRepository ImageRepository
        {
            get
            {
                return _imageRepository ??= new ImageRepository(_context);
            }
        }

        public ITransactionLogRepository TransactionLogRepository
        {
            get
            {
                return _transactionLog ??= new TransactionLogRepository(_context);
            }
        }

        public IVoucherRepository VoucherRepository
        {
            get
            {
                return _voucherRepository ??= new VoucherRepository(_context);
            }
        }

        public IVoucherUserRepository VoucherUserRepository
        {
            get
            {
                return _voucherUserRepository ??= new VoucherUserRepository(_context);
            }
        }

        public IManagerVoucherLogRepository ManagerVoucherLogRepository
        {
            get
            {
                return _managerVoucher ??= new ManagerVoucherLogRepository(_context);
            }
        }

        public IUserTokenRepository UserTokenRepository
        {
            get
            {
                return _userTokenRepository ??= new UserTokenRepository(_context);
            }
        }
        public void Commit()
            => _context.SaveChanges();
        public async Task CommitAsync()
            => await _context.SaveChangesAsync();
        public void Rollback()
            => _context.Dispose();

        public async Task RollbackAsync()
            => await _context.DisposeAsync();
    }
}
