namespace ForumProject.Application.Features.Users.DTOs
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set;}
        public DateTime? DeactivatedAt { get; set; }
    }
}