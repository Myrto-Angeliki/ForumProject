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

        public Comment Map()
        {
            Comment comment = new Comment
            {
                CommentId = this.CommentId,
                PostId = this.PostId,
                UserId = this.UserId,
                Content = this.Content,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt
            };
            return comment;
        }

        public static IEnumerable<CommentDto> Map(IEnumerable<Comment> comments)
        {
            List<CommentDto> commentDtos = new List<CommentDto>();
            foreach(Comment comment in comments)
            {
                CommentDto commentDto = new CommentDto{
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    UpdatedAt = comment.UpdatedAt
                };
                commentDtos.Add(commentDto);
            }

            return commentDtos;
        }
    }
}