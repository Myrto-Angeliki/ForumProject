namespace ForumProject.Application.Features.Users.DTOs
{
    public class UpdateEmailDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
    }
}