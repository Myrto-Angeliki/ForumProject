namespace ForumProject.Application.Features.Users.DTOs
{
    public class UpdateUsernameDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
    }
}