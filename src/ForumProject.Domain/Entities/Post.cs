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
            UpdatedAt = DateTime.UtcNow;
        }
        public void DeleteComment(Comment comment)
        {
            Comments.Remove(comment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTopic(Topic topic)
        {
            if(PostTopics.Count() == 15)
            {
                throw new Exception("A post cannot have more than 15 topics!");
            }
            PostTopics.Add(topic);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveTopic(Topic topic)
        {
            if(PostTopics.Count() == 1)
            {
                throw new Exception("A post must have at least one topic!");
            }
            PostTopics.Remove(topic);
            UpdatedAt = DateTime.UtcNow;
        }

        public void CheckTitleNotEmpty()
        {
            if(Title == "")
            {
                throw new Exception("Title content cannot be empty!");
            }
        }

        public void CheckContentNotEmpty()
        {
            if(Content == "")
            {
                throw new Exception("Post content cannot be empty!");
            }
        }

        public void CheckAtLeastOneTopic()
        {
            if(PostTopics.Count() == 0)
            {
                throw new Exception("A post must have at least one topic!");
            }
        }
    }
}