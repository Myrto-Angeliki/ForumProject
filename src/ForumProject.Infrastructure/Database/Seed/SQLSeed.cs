using Microsoft.Extensions.Configuration;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Database.Seed
{
    public class SQLSeed
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("src\\ForumProject.Api\\appsettings.json")
                    .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

            DataContextDapper context = new DataContextDapper(connectionString);

            using(var connection = context.CreateConnection())
            {
                
            }
        }
    }
}