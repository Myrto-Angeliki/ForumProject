using System.Reflection.Metadata;

namespace ForumProject.Domain.Entities
{
    public class FriendRequest
    {
        public User Sender { get; set; }
        public User Recipient { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public FriendRequest(User sender, User recipient, 
            DateTime? updatedAt = null)
        {
            Sender = sender;
            Recipient = recipient;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = updatedAt;
        }

        
    }
}