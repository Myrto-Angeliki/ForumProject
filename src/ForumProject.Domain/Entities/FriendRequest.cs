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

        // public void UpdateUserFriendRequestLists(string action)
        // {
        //     if(action.ToLower() == "add")
        //     {
        //         this.Sender.FriendRequestsSent.Add(this);
        //         this.Recipient.FriendRequestsReceived.Add(this);
        //     }
        //     else if(action.ToLower() == "remove")
        //     {
        //         this.Sender.FriendRequestsSent.Remove(this);
        //         this.Recipient.FriendRequestsReceived.Remove(this);
        //     }
        //     else
        //         throw new Exception("Unsupported action " + action + "!");
            
        //     this.Sender.UpdatedAt = DateTime.UtcNow;
        //     this.Recipient.UpdatedAt = DateTime.UtcNow;
        // }

        // public static FriendRequest? FindFriendRequest(User sender, User recipient, 
        //     List<FriendRequest> listToSearchIn)
        // {
        //     return listToSearchIn.Find((friendRequest) =>
        //     {
        //         return friendRequest.Sender.UserId == sender.UserId 
        //                 && friendRequest.Recipient.UserId == recipient.UserId;
        //     });
        // }
    }
}