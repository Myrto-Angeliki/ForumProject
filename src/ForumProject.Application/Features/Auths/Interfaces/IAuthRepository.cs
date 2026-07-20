using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Auths.Interfaces
{
    public interface IAuthRepository
    {
        Task<IEnumerable<Auth>> GetAllAsync();
        Task<Auth?> GetByEmailAsync(string email);
        Task<bool> AddAsync(Auth auth);
        Task<bool> UpdateAsync(Auth auth);
        Task<bool> DeleteAsync(string email);
    }
}