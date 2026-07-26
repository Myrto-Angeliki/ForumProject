using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.DTOs
{
    public class FriendRequestDto
    {
        public int SenderId { get; set; } = new();
        public int RecipientId { get; set; } = new();
        public string Action { get; set; } = "";
    }
}