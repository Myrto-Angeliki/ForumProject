using Microsoft.Extensions.Configuration;
using ForumProject.Infrastructure.Persistence;
using System.Data;
using Dapper;
using System.Text;

namespace ForumProject.Seeder
{
    public class Program
    {
        static void ResetTables(DataContextDapper context)
        {
            using(var connection = context.CreateConnection())
            {
                connection.Execute("DBCC CHECKIDENT('ForumAppSchema.Users', RESEED, 0)");
                connection.Execute("DBCC CHECKIDENT('ForumAppSchema.Topics', RESEED, 0)");
                connection.Execute("DBCC CHECKIDENT('ForumAppSchema.Posts', RESEED, 0)");
                connection.Execute("DBCC CHECKIDENT('ForumAppSchema.Comments', RESEED, 0)");
                
                connection.Execute("ForumAppSchema.spUser_Delete", 
                    commandType: CommandType.StoredProcedure);
                connection.Execute("ForumAppSchema.spTopic_Delete", 
                    commandType: CommandType.StoredProcedure);
            }
        }
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("C:\\Users\\user\\source\\repos\\ForumProject\\src\\ForumProject.Api\\appsettings.json")
                    .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

            DataContextDapper context = new DataContextDapper(connectionString);

            using(var connection = context.CreateConnection())
            {
                Random rand = new Random();

                ResetTables(context);

                for(int i=1; i<=500; i++)
                {
                    var theBytes = Encoding.UTF8.GetBytes("pass"+i.ToString());
                    var registerValues = new {  Email = "user"+i+"@seedmail.com",
                                                PasswordHash = theBytes,
                                                PasswordSalt = theBytes};
                    connection.Execute("ForumAppSchema.spRegistration_Upsert", registerValues
                                        , commandType: CommandType.StoredProcedure);

                    var userValues = new {  Username = "Username_"+i,
                                            Email = "user"+i+"@seedmail.com",
                                            IsActive = 1
                                        };
                    connection.Execute("ForumAppSchema.spUser_Upsert", userValues
                                        , commandType: CommandType.StoredProcedure);
                    

                    var topicValues = new { TopicName = "Topic"+i};
                    connection.Execute("ForumAppSchema.spTopic_Upsert", topicValues
                                        , commandType: CommandType.StoredProcedure);
                }

                for(int i=1; i<=1000; i++)
                {
                    var postValues = new {  UserId = rand.Next(1, 501),  
                                            Title = "Title_"+i,
                                            Content = "Post_"+i+" text content",
                                            FeaturedImage = "Post_"+i+".png"
                                        };
                    connection.Execute("ForumAppSchema.spPost_Upsert", postValues
                                        , commandType: CommandType.StoredProcedure);

                    var senderId = rand.Next(1, 501);
                    var recipientId = rand.Next(1, 501);
                    while(recipientId == senderId)
                    {
                        recipientId = rand.Next(1, 501);
                    }

                    var friendRequestValues = new { SenderId = senderId, 
                                                    RecipientId = recipientId};
                    connection.Execute("ForumAppSchema.spFriendRequest_Insert", friendRequestValues
                                        , commandType: CommandType.StoredProcedure);
                }

                for(int i=1; i<=500; i++)
                {
                    var commentValues = new {   UserId = rand.Next(1, 501), 
                                                PostId = rand.Next(1, 1001),
                                                Content = "Comment_"+i+" text content",
                                        };
                    connection.Execute("ForumAppSchema.spComment_Upsert", commentValues
                                        , commandType: CommandType.StoredProcedure);
                }
            }
        }
    }
}