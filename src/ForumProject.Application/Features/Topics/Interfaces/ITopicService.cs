using ForumProject.Application.Features.Topics.DTOs;

namespace ForumProject.Application.Features.Topics.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<TopicDto>> GetAllAsync();
        Task<TopicDto> GetByIdAsync(int topicId);
        Task<bool> AddAsync(TopicDto topicDto);
        Task<IEnumerable<TopicDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<TopicDto>> GetByPostIdAsync(int postId);
        Task<bool> DeleteAsync(int topicId);

    }
}