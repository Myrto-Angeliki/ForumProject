using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Mappers;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Services
{
    public class PostServiceHelper
    {
        public static Post GetPostToUpsert(PostDto postDto, bool isInsert)
        {
            Post post = PostMapper.MapToPost(postDto);
            if(isInsert) post.PostId = 0;
            post.CheckTitleNotEmpty();
            //post.CheckAtLeastOneTopic();
            post.CheckContentNotEmpty();
            return post;
        }
    }
}