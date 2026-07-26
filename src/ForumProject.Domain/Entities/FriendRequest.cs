using System.Reflection.Metadata;

namespace ForumProject.Domain.Entities
{
    public class FriendRequest
    {
        private readonly User _sender;
        public User Sender => _sender;

        private readonly User _recipient;
        public User Recipient => _recipient;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public FriendRequest(User sender, User recipient)
        {
            _sender = sender;
            _recipient = recipient;
        }

        public void UpdateUserFriendRequestLists(string action)
        {
            if(action.ToLower() == "add")
            {
                this.Sender.FriendRequestsSent.Add(this);
                this.Recipient.FriendRequestsReceived.Add(this);
            }
            else if(action.ToLower() == "remove")
            {
                this.Sender.FriendRequestsSent.Remove(this);
                this.Recipient.FriendRequestsReceived.Remove(this);
            }
            else
                throw new Exception("Unsupported acition " + action + "!");
            
            this.Sender.UpdatedAt = DateTime.UtcNow;
            this.Recipient.UpdatedAt = DateTime.UtcNow;
        }

        public static FriendRequest? FindFriendRequest(User sender, User recipient, 
            List<FriendRequest> listToSearchIn)
        {
            return listToSearchIn.Find((friendRequest) =>
            {
                return friendRequest.Sender.UserId == sender.UserId 
                        && friendRequest.Recipient.UserId == recipient.UserId;
            });
        }
    }
}