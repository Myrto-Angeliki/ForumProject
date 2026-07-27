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
        public List<FriendRequest> FriendRequestsSent { get; set; } = [];
        public List<FriendRequest> FriendRequestsReceived { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }


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
                this.Friends.Add(friend);
                friend.Friends.Add(this);
            }
            else if(action.ToLower() == "remove")
            {
                this.Friends.Remove(friend);
                friend.Friends.Remove(this);
            }
            else
                throw new Exception("Invalid action: " 
                        + action + ".");

            this.UpdatedAt = DateTime.UtcNow;
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

        public void EmptyFriendRequestsList(bool isSender)
        {
            if(isSender)
                    FriendRequestsSent = new();
                else 
                    FriendRequestsReceived = new();
        }
        
        public void CheckIfFriendRequestExists(User recipient)
        {
            
            FriendRequest? friendRequestFromRecipient = FriendRequest.FindFriendRequest(
                sender: recipient, recipient: this, recipient.FriendRequestsSent);
            if (friendRequestFromRecipient != null)
            {
                throw new Exception(@"Failed to send friend request. 
                    User" + recipient.Username + " has already sent you a friend request.");
            }

        }

        // public void HandleReceivedFriendRequest(User sender, string action)
        // {
        //     FriendRequest? friendRequest = FriendRequest.FindFriendRequest(
        //         sender, this, this.FriendRequestsReceived
        //     );
        //     if(friendRequest != null)
        //     {
        //         friendRequest.UpdateUserFriendRequestLists("remove");
        //         if(action == "accept")
        //             UpdateFriends(sender, "add");
        //     }
        //     throw new Exception("Friend Request not found.");
        // }

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