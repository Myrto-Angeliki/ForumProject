namespace ForumProject.Application.Features.Posts.DTOs
{
    public class TopicOfPostDto
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; } = "";
    }
}