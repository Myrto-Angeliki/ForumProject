using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Domain.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContextDapper _context;

        public AuthRepository(DataContextDapper context)
        {
            _context = context;
        }

        private async Task<bool> UpsertAsync(Auth auth)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "ForumAppSchema.spRegistration_Upsert"
                    , new { Email=auth.Email, PasswordHash=auth.PasswordHash
                            , PasswordSalt=auth.PasswordSalt}
                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> AddAsync(Auth auth)
        {
            return await UpsertAsync(auth);
        }

        public async Task<bool> DeleteAsync(string email)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "ForumAppSchema.spRegistration_Delete"
                    , new {Email = email}
                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Auth>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Auth> authenticatedUsers = await connection.QueryAsync<Auth>(
                    "ForumAppSchema.spLoginConfirmation_Get"
                    , commandType: CommandType.StoredProcedure
                );
                return authenticatedUsers;
            }
        }

        public async Task<Auth?> GetByEmailAsync(string email)
        {
            using(var connection = _context.CreateConnection())
            {
                Auth? authenticatedUser = await connection.QuerySingleAsync<Auth>(
                    "ForumAppSchema.spLoginConfirmation_Get"
                    , new {Email = email}
                    , commandType: CommandType.StoredProcedure
                );
                return authenticatedUser;
            }
        }

        public async Task<bool> UpdateAsync(Auth auth)
        {
            return await UpsertAsync(auth);
        }
    }
}