namespace ForumProject.Domain
{
    public class Comment
    {
        public int CommentId { get; set; }
        private readonly int _postId;
        public int PostId => _postId;
        private readonly int _userId;
        public int UserId => _userId;
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}