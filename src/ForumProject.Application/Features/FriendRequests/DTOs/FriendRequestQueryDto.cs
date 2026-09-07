namespace ForumProject.Application.Features.FriendRequests.DTOs
{
    public class FriendRequestQueryDto
{
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = "";
    public string SenderEmail { get; set; } = "";
    public bool SenderIsActive { get; set; }
    public DateTime SenderCreatedAt { get; set; }
    public DateTime SenderUpdatedAt { get; set; }
    public DateTime? SenderDeactivatedAt { get; set; }

    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; } = "";
    public string RecipientEmail { get; set; } = "";
    public bool RecipientIsActive { get; set; }
    public DateTime RecipientCreatedAt { get; set; }
    public DateTime RecipientUpdatedAt { get; set; }
    public DateTime? RecipientDeactivatedAt { get; set; }

    public DateTime FriendRequestCreatedAt { get; set; }
    public DateTime? FriendRequestUpdatedAt { get; set; }
}
}