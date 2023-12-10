using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class BranchService : IBranchService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        public BranchService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task Add(BranchInfoVM branchInfo, long userId, int districtId)
        {
            Branch branch = new()
            {
                BranchName = branchInfo.BranchName,
                DateCreate = DateTime.UtcNow.ToTimeZone(),
                DateUpdate = DateTime.UtcNow.ToTimeZone(),
                DistrictId = districtId,
                IsRemoved = false,
                UrlGoogleMap = branchInfo.UrlGoogleMap,
                UserIdCreate = userId,
                UserIdUpdate = userId,
            };
            ICollection<ImageModel> imgModel = new List<ImageModel>();
            if (branchInfo.uploadLst != null)
            {
                imgModel = await _imageService.AddRange(branchInfo.uploadLst, userId);
            }    
            branch.Imagelist = imgModel.ToList();
            _unitOfWork.BranchRepository.Add(branch);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<ProvinceBranchVM>> GetListWithBranchAsync()
        {
            var provincelst = await _unitOfWork.ProvinceRepository.GetAllAsync(null, x => x.Include(d => d.Districts!));
            var branchlst = await _unitOfWork.BranchRepository.GetAllAsync(null, x => x.Include(x => x.Imagelist!));
            ICollection<ProvinceBranchVM> model = new List<ProvinceBranchVM>();
            foreach (var province in provincelst)
            {
                var districtlst = province.Districts!.Select(x => x.Id).ToList();
                var lstitem = branchlst.Where(x => districtlst.Contains(x.DistrictId)).ToList();
                var item = new ProvinceBranchVM()
                {
                    ProvinceId = province.Id,
                    ProvinceName = province.CityNameShort ?? "",
                    BranchCount = lstitem.Count,
                    branches = lstitem.Select(x => new BranchInfoVM(x)).ToList(),
                    districts = province.Districts!.Select(x => new DistrictInfoVM(x)).ToList()
                };
                model.Add(item);
            }
            return model;
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var branch = await _unitOfWork.BranchRepository.GetAsync(x => x.Id == Id);
            if (branch == null) return false;
            branch.IsRemoved = true;
            branch.UserIdRemove = userId;
            branch.DateRemove = DateTime.UtcNow.ToTimeZone();
            _unitOfWork.BranchRepository.Update(branch);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Branch? GetById(int Id)
            => _unitOfWork.BranchRepository.Get(x => x.Id == Id);

        public async Task<Branch?> GetByIdAsync(int Id)
            => await _unitOfWork.BranchRepository.GetAsync(x => x.Id == Id);

        public async Task<Branch?> GetByIdAsync(int Id, Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes)
            => await _unitOfWork.BranchRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<Branch>> GetListAsync()
            => await _unitOfWork.BranchRepository.GetAllAsync();

        public async Task<ICollection<Branch>> GetListAsync(int? districtId, int? provinceId)
        {
            ICollection<Branch> branches = new List<Branch>();
            if(districtId.HasValue)
            {
                branches = await _unitOfWork.BranchRepository.GetAllAsync(x => x.DistrictId == districtId.Value);
            } 
            else if(provinceId.HasValue)
            {
                var districts = await _unitOfWork.DistrictRepository.GetAllAsync(x => x.ProvinceId == provinceId.Value);
                var districtIdlst = districts.Select(x => x.Id).ToList();
                branches =  await _unitOfWork.BranchRepository.GetAllAsync(x => districtIdlst.Contains(x.DistrictId));
            }
            return branches;
        }

        public async Task<ICollection<Branch>> GetListAsync(int? districtId, int? provinceId, Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes)
        {
            ICollection<Branch> branches = new List<Branch>();
            if (districtId.HasValue)
            {
                branches = await _unitOfWork.BranchRepository.GetAllAsync(x => x.DistrictId == districtId.Value, includes);
            }
            else if (provinceId.HasValue)
            {
                var districts = await _unitOfWork.DistrictRepository.GetAllAsync(x => x.ProvinceId == provinceId.Value);
                var districtIdlst = districts.Select(x => x.Id).ToList();
                branches = await _unitOfWork.BranchRepository.GetAllAsync(x => districtIdlst.Contains(x.DistrictId), includes);
            }
            return branches;
        }

        public async Task<ICollection<Branch>> GetListAsync(Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes)
            => await _unitOfWork.BranchRepository.GetAllAsync(null, includes);

        public async Task Update(BranchInfoVM branchInfo, long userId)
        {
            var branch = await _unitOfWork.BranchRepository.GetAsync(x => x.Id == branchInfo.BranchId);
            if(branch != null)
            {
                branch.UrlGoogleMap = branchInfo.UrlGoogleMap;
                branch.BranchName = branchInfo.BranchName;
                branch.DateUpdate = DateTime.UtcNow.ToTimeZone();

            }
        }
    }
}
