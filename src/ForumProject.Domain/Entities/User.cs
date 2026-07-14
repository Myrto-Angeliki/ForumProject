namespace ForumProject.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; }
        public List<Topic> FollowingTopics { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<User> Friends { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime UserDeactivatedAtId { get; set; }
        
        
        private readonly List<FriendRequest> _friendRequestsSent = new();
        public IReadOnlyCollection<FriendRequest> FriendRequestsSent => _friendRequestsSent;

        private readonly List<FriendRequest> _friendRequestsReceived = new();
        public IReadOnlyCollection<FriendRequest> FriendRequestsReceived => _friendRequestsReceived;

        
        public void AddPost(Post post)
        {
            Posts.Add(post);
        }

        public void RemovePost(Post post)
        {
            Posts.Remove(post);
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }

        public void RemoveComment(Comment comment)
        {
            Comments.Remove(comment);
        }

        public void AddFriend(User friendToAdd)
        {
            Friends.Add(friendToAdd);
        }

        public void RemoveFriend(User friendToRemove)
        {
            Friends.Remove(friendToRemove);
        }

        public void AddTopic(Topic topicToFollow)
        {
            FollowingTopics.Add(topicToFollow);
        }

        public void RemoveTopic(Topic topicToUnfollow)
        {
            FollowingTopics.Remove(topicToUnfollow);
        }
        
        public void AddFriendRequest(User recipient)
        {
            foreach(FriendRequest friendRequest in recipient.FriendRequestsSent)
            {
                if(friendRequest.Recipient.UserId == this.UserId)
                {
                    throw new Exception(@"Failed to send friend request. 
                        User" + recipient.Username + " has already sent you a friend request.");
                }
            }
            FriendRequest newFriendRequest = new FriendRequest(this, recipient);
            _friendRequestsSent.Add(newFriendRequest);
            recipient._friendRequestsReceived.Add(newFriendRequest);
        }

    }
}