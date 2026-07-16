namespace ForumProject.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set;}
        public List<Topic> FollowingTopics { get; set; } = [];
        public List<Post> Posts { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
        public List<User> Friends { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }


        private readonly List<FriendRequest> _friendRequestsSent = [];
        public IReadOnlyCollection<FriendRequest> FriendRequestsSent => _friendRequestsSent;

        private readonly List<FriendRequest> _friendRequestsReceived = [];
        public IReadOnlyCollection<FriendRequest> FriendRequestsReceived => _friendRequestsReceived;


        public void UpdatePosts(Post post, string action)
        {
            if(action.ToLower() == "add")
                Posts.Add(post);
            else if(action.ToLower() == "remove")
                Posts.Remove(post);
            else
                throw new Exception("Invalid action: " 
                        + action + ".");

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateComments(Comment comment, string action)
        {
            if(action.ToLower() == "add")
                Comments.Add(comment);
            else if(action.ToLower() == "remove")
                Comments.Remove(comment);
            else
                throw new Exception("Invalid action: " 
                        + action + ".");

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateFriends(User friend, string action)
        {
            if(action.ToLower() == "add")
            {
                Friends.Add(friend);
                friend.Friends.Add(this);
            }
            else if(action.ToLower() == "remove")
            {
                Friends.Remove(friend);
                friend.Friends.Remove(this);
            }
            else
                throw new Exception("Invalid action: " 
                        + action + ".");

            UpdatedAt = DateTime.UtcNow;
            friend.UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTopics(Topic topic, string action)
        {
            if(action.ToLower() == "add")
            {
                FollowingTopics.Add(topic);
                topic.UsersFollowingTopic.Add(this);
            }
            else if(action.ToLower() == "remove")
            {
                FollowingTopics.Remove(topic);
                topic.UsersFollowingTopic.Remove(this);
            }
            else
                throw new Exception("Cannot update following topics with action: " 
                        + action + ".");

            UpdatedAt = DateTime.UtcNow;
            topic.UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateFriendRequests(FriendRequest friendRequest, 
            bool isSender, string action)
        {
            if(isSender)
            {
                if(action.ToLower() == "add")
                    _friendRequestsSent.Add(friendRequest);
                else if(action.ToLower() == "remove")
                    _friendRequestsSent.Remove(friendRequest);
                else
                    throw new Exception("Cannot modify sent friend requests with action: " 
                        + action + ".");    
            }
            else
            {
                if(action.ToLower() == "add")
                    _friendRequestsReceived.Add(friendRequest);
                else if(action.ToLower() == "remove")
                    _friendRequestsReceived.Remove(friendRequest);
                else
                    throw new Exception("Cannot modify received friend requests with action: " 
                        + action + ".");
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void SendFriendRequest(User recipient)
        {
            foreach (FriendRequest friendRequest in recipient.FriendRequestsSent)
            {
                if (friendRequest.Recipient.UserId == this.UserId)
                {
                    throw new Exception(@"Failed to send friend request. 
                        User" + recipient.Username + " has already sent you a friend request.");
                }
            }
            FriendRequest newFriendRequest = new FriendRequest(this, recipient);
            UpdateFriendRequests(newFriendRequest, true, "Add");
            recipient.UpdateFriendRequests(newFriendRequest, false, "Add");
        }

        public FriendRequest? FindReceivedFriendRequest(User sender)
        {
            return _friendRequestsReceived.Find((friendRequest) =>
            {
                return friendRequest.Sender.UserId == sender.UserId 
                        && friendRequest.Recipient.UserId == UserId;
            });
        }

        public void HandleReceivedFriendRequest(User sender, string action)
        {
            FriendRequest? friendRequest = FindReceivedFriendRequest(sender);
            if(friendRequest != null)
            {
                UpdateFriendRequests(friendRequest, false, action);
                sender.UpdateFriendRequests(friendRequest, true, action);
                UpdateFriends(sender, action);
            }
            throw new Exception("Friend Request not found.");
        }

        public void UpdateUserAcitvity(bool isDeactivation)
        {
            IsActive = isDeactivation ? false : true;
            DeactivatedAt = isDeactivation ? DateTime.UtcNow : null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeEmail(IEnumerable<User> users, string newEmail)
        {
            foreach(User user in users)
            {
                if(user.Email == newEmail)
                {
                    throw new Exception(@"Failed to change email. 
                        A registered account already exists with this email '" + newEmail + "'.");
                }
            }
            Email = newEmail;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeUsername(IEnumerable<User> users, string newUsername)
        {
            foreach(User user in users)
            {
                if(user.Username == newUsername)
                {
                    throw new Exception(@"Failed to change username. 
                        Username '" + newUsername + "' already exists.");
                }
            }
            Username = newUsername;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}