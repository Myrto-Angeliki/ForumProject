using System.Data;
using Dapper;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextDapper _context;

        public UserRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(User user)
        {
            using(var connection = _context.CreateConnection())
            {
                var userParams = new {Email = user.Email, Username = user.Username};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_Upsert", userParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                var userParams = new {UserIdParam = userId};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_Delete", userParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<User> users = await connection.QueryAsync<User>(
                                            "ForumAppSchema.spUser_Get"
                                            , commandType: CommandType.StoredProcedure);
                return users ;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using(var connection = _context.CreateConnection())
            {
                User? user = await connection.QuerySingleAsync<User>(
                                            "ForumAppSchema.spUser_Get"
                                            , new {Email = email}
                                            , commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                User? user = await connection.QuerySingleAsync<User>(
                                            "ForumAppSchema.spUser_Get"
                                            , new {UserId = userId}
                                            , commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using(var connection = _context.CreateConnection())
            {
                User? user = await connection.QuerySingleAsync<User>(
                                            "ForumAppSchema.spUser_Get"
                                            , new {Username = username}
                                            , commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using(var connection = _context.CreateConnection())
            {
                var userParams = new {  UserId = user.UserId,
                                        Username = user.Username,
                                        Email = user.Email,
                                        IsActive = user.IsActive,
                                        DeactivatedAt = user.DeactivatedAt};
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_Upsert", userParams
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}