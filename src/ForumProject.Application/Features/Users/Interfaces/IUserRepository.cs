using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);

        Task<bool> AddAsync(User user);
        Task<bool> UpdateStatusAsync(User user);
        Task<bool> UpdateEmailAsync(User user);
        Task<bool> UpdateUsernameAsync(User user);
        Task<bool> DeleteAsync(int userId);
    }
}