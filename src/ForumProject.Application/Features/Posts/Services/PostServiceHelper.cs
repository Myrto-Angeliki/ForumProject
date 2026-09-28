using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Services
{
    public class PostServiceHelper
    {
        public static void GetPostToUpsert(Post post)
        {
            post.CheckTitleNotEmpty();
            //post.CheckAtLeastOneTopic();
            post.CheckContentNotEmpty();
        }
    }
}