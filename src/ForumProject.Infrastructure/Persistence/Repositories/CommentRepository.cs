using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContextDapper _context;

        public CommentRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Comment comment)
        {
            using(var connection = _context.CreateConnection())
            {
                var commentParams = new {   UserId = comment.UserId, PostId = comment.@PostId,
                                            Content = comment.Content};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spComment_Upsert"
                                            , commentParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spComment_Delete"
                                            , new {CommentId = commentId}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Comment> comments = await connection.QueryAsync<Comment>(
                                            "ForumAppSchema.spComment_Get"
                                            , commandType: CommandType.StoredProcedure);
                return comments ;
            }
        }

        public async Task<Comment?> GetByIdAsync(int commentId)
        {
            using(var connection = _context.CreateConnection())
            {
                Comment? comment = await connection.QuerySingleOrDefaultAsync<Comment>(
                                            "ForumAppSchema.spComment_Get"
                                            , new {CommentId = commentId}
                                            , commandType: CommandType.StoredProcedure);
                return comment;
            }
        }

        public async Task<IEnumerable<Comment>> GetByPostAsync(int postId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Comment> comments = await connection.QueryAsync<Comment>(
                                            "ForumAppSchema.spComment_Get"
                                            , new {PostId = postId}
                                            , commandType: CommandType.StoredProcedure);
                return comments;
            }
        }

        public async Task<IEnumerable<Comment>> GetByUserAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Comment> comments = await connection.QueryAsync<Comment>(
                                            "ForumAppSchema.spComment_Get"
                                            , new {UserId = userId}
                                            , commandType: CommandType.StoredProcedure);
                return comments;
            }
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            using(var connection = _context.CreateConnection())
            {
                var commentParams = new {   PostId = comment.PostId, 
                                            UserId = comment.UserId,
                                            CommentId = comment.CommentId,
                                            Content = comment.Content};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spComment_Upsert"
                                            , commentParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}
