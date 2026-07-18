using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Mappers
{
    public class PostMapper
    {
        public static Post MapToPost(PostDto postDto)
        {
            Post post = new Post
            {
                PostId = postDto.PostId,
                UserId = postDto.UserId,
                Title = postDto.Title,
                Content = postDto.Content,
                FeaturedImage = postDto.FeaturedImage,
                PostTopics = postDto.PostTopics,
                Comments = postDto.Comments,
                CreatedAt = postDto.CreatedAt,
                UpdatedAt = postDto.UpdatedAt
            };

            return post;
        }

        public static PostDto MapToPostDto(Post post)
        {
            PostDto postDto = new PostDto
            {
                PostId = post.PostId,
                UserId = post.UserId,
                Title = post.Title,
                Content = post.Content,
                FeaturedImage = post.FeaturedImage,
                PostTopics = post.PostTopics,
                Comments = post.Comments,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
            return postDto;
        }

        public static IEnumerable<PostDto> MapToPostDtos(IEnumerable<Post> posts)
        {
            List<PostDto> postDtos = [];
            foreach(Post post in posts)
            {
                postDtos.Add(MapToPostDto(post));
            }
            return postDtos;
        }
    }
}