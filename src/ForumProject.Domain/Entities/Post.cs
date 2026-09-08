using ForumProject.Domain.Exceptions;

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


        public void AddTopic(Topic topic)
        {
            if(PostTopics.Count() == 15)
                throw new InvalidStateException("A post cannot have more than 15 topics!");
        }

        public void RemoveTopic()
        {
            if(PostTopics.Count() == 1)
                throw new InvalidStateException("A post must have at least one topic!");
        }

        public void CheckTitleNotEmpty()
        {
            if(Title == String.Empty)
                throw new InvalidStateException("Post title cannot be empty!");
        }

        public void CheckContentNotEmpty()
        {
            if(Content == String.Empty)
                throw new InvalidStateException("Post content cannot be empty!");
        }

        public void CheckAtLeastOneTopic()
        {
            if(PostTopics.Count() == 0)
                throw new InvalidStateException("A post must have at least one topic!");
        }
    }
}