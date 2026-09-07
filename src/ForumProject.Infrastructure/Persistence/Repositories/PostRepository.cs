using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContextDapper _context;

        public PostRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Post post)
        {
            using(var connection = _context.CreateConnection())
            {
                var postParams = new {  UserId = post.UserId, Title = post.Title,
                                        Content = post.Content,
                                        FeaturedImage = post.FeaturedImage};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spPost_Upsert"
                                            , postParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int postId)
        {
            using(var connection = _context.CreateConnection())
            {
                var postParams = new {@PostIdDel = postId};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spPost_Delete", postParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Post> posts = await connection.QueryAsync<Post>(
                                            "ForumAppSchema.spPost_Get"
                                            , commandType: CommandType.StoredProcedure);
                return posts ;
            }
        }

        public async Task<Post?> GetByIdAsync(int postId)
        {
            using(var connection = _context.CreateConnection())
            {
                Post? post = await connection.QuerySingleOrDefaultAsync<Post>(
                                            "ForumAppSchema.spPost_Get"
                                            , new {PostId = postId}
                                            , commandType: CommandType.StoredProcedure);
                return post;
            }
        }

        public async Task<IEnumerable<Post>> GetByUserAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Post> posts = await connection.QueryAsync<Post>(
                                            "ForumAppSchema.spPost_Get"
                                            , new {UserId = userId}
                                            , commandType: CommandType.StoredProcedure);
                return posts;
            }
        }

        public async Task<bool> UpdateAsync(Post post)
        {
            using(var connection = _context.CreateConnection())
            {
                var postParams = new {  PostId = post.PostId, 
                                        UserId = post.UserId,
                                        Title = post.Title,
                                        Content = post.Content,
                                        FeaturedImage = post.FeaturedImage};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spPost_Upsert"
                                            , postParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}