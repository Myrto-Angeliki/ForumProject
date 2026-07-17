using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.DTOs
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string FeaturedImage { get; set; } = "";
        public List<Topic> PostTopics { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}