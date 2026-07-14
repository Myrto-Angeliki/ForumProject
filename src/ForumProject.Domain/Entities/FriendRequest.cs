namespace ForumProject.Domain.Entities
{
    public class FriendRequest
    {
        private readonly User _sender;
        public User Sender => _sender;

        private readonly User _recipient;
        public User Recipient => _recipient;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public FriendRequest(User sender, User recipient)
        {
            _sender = sender;
            _recipient = recipient;
        }
    }
}