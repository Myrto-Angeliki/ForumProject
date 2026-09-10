using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Comments.DTOs
{
    public class AddCommentDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = "";
    }
}