namespace ForumProject.Application.Features.Users.DTOs
{
    public class UpdateStatusDto
    {
        public int UserId { get; set; }
        public bool? IsActive { get; set;}
    }
}