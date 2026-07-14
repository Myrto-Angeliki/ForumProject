using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface ITopicRepository : IDisposable
    {
        Task<IEnumerable<Topic>> GetTopicsAsync();
        Task<IEnumerable<Topic>> GetTopicsByUserIdAsync(int userId);
        Task<IEnumerable<Topic>> GetTopicsByPostIdAsync(int postId);
        Task<Topic?> GetTopicByIdAsync(int postId);
        Task<Topic?> GetTopicByTopicNameAsync(string topicName);
        Task AddTopicAsync(Topic post);
        Task UpdateTopicAsync(Topic post);
        Task DeleteTopicAsync(Topic post);
        Task DeleteTopicByIdAsync(int topicId);
        Task DeleteTopicByName(string topicName);
    }
}