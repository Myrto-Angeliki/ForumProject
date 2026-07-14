namespace ForumProject.Domain.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string FeaturedImage { get; set; } = "";
        public List<Topic> PostTopics { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }

        public void AddTopic(Topic topic)
        {
            PostTopics.Add(topic);
        }
    }
}