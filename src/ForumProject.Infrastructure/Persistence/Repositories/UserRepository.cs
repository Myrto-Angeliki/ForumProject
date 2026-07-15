using System.Runtime.CompilerServices;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Domain.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly DataContextDapper _context;

        public UserRepository(DataContextDapper context)
        {
            _context = context;
        }
    }
}