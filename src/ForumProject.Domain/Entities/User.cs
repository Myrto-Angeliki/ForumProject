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


        
    }
}