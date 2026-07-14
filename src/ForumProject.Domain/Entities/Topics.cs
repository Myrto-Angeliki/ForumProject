namespace ForumProject.Domain
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; } = "";
        public List<User> UsersFollowingTopic { get; set; } = new();
        public List<Post> PostsWithTopic { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}