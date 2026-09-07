using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Comments.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}