using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Services
{
    public class PostServiceHelper
    {
        public static void GetPostToUpsert(Post post, bool isInsert)
        {
            if(isInsert) post.PostId = 0;
            post.CheckTitleNotEmpty();
            //post.CheckAtLeastOneTopic();
            post.CheckContentNotEmpty();
        }
    }
}