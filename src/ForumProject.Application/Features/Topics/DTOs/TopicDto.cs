namespace ForumProject.Application.Features.Topics.DTOs
{
    public class TopicDto
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}