using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic?> GetByIdAsync(int topicId);
        Task<IEnumerable<Topic>> GetByUserAsync(int userId);
        Task<IEnumerable<Topic>> GetByPostAsync(int postId);

        Task<bool> AddAsync(Topic topic);
        Task<bool> UpdateAsync(Topic topic);
        Task<bool> DeleteAsync(int topicId);
    }
}