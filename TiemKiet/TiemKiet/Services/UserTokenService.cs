using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class UserTokenService : IUserTokenService
    {
        public IUnitOfWork _unitOfWork;
        public UserTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;    
        }
        public async Task Add(long? userId, string token)
        {
            var valid = await _unitOfWork.UserTokenRepository.GetAsync(x => x.UserId == userId && x.UserToken == token);
            if(valid == null)
            {
                ApplicationUserToken model = new()
                {
                    UserId = userId,
                    UserToken = token
                };
                _unitOfWork.UserTokenRepository.Add(model);
                await _unitOfWork.CommitAsync();
            }    
        }

        public Task<bool> Delete(int Id, long userId)
        {
            throw new NotImplementedException();
        }

        public ApplicationUserToken? GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserToken?> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserToken?> GetByIdAsync(int Id, Func<IQueryable<ApplicationUserToken>, IIncludableQueryable<ApplicationUserToken, object>> includes)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ApplicationUserToken>> GetListAsync(long? userId)
        {
            if(userId.HasValue)
            {
                return await _unitOfWork.UserTokenRepository.GetAllAsync(x => x.UserId == userId);
            }
            else
            {
                return await _unitOfWork.UserTokenRepository.GetAllAsync();
            }
        }

        public Task<ICollection<ApplicationUserToken>> GetListAsync(long? userId, Func<IQueryable<ApplicationUserToken>, IIncludableQueryable<ApplicationUserToken, object>> includes)
        {
            throw new NotImplementedException();
        }
    }
}
