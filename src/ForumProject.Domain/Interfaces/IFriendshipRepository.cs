using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByUserAsync(int userId);
        Task<bool> AddAsync(int userId1, int userId2);
        Task<bool> DeleteAsync(int userId1, int userId2);
        Task<bool> DeleteByUserAsync(int userId);
    }
}