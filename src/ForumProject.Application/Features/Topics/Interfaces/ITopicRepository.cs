using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Topics.Interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic?> GetByIdAsync(int topicId);
        Task<IEnumerable<Topic>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Topic>> GetByPostIdAsync(int postId);
        Task<IEnumerable<Post>> GetPostsByTopicIdAsync(int topicId);
        Task<IEnumerable<User>> GetUsersByTopicIdAsync(int topicId);

        Task<bool> AddAsync(Topic topic);
        Task<bool> UpdateAsync(Topic topic);
        Task<bool> DeleteAsync(int topicId);
    }
}