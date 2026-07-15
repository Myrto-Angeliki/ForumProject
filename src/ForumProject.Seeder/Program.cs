using Microsoft.Extensions.Configuration;
using ForumProject.Infrastructure.Persistence;
using Dapper;

namespace ForumProject.Seeder
{
    public class Program
    {
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

                // for(int i=1; i<=500; i++)
                // {
                //     var userValues = new {  Username = "Username_"+i,
                //                             Email = "user"+i+"@seedmail.com",
                //                             IsActive = 1
                //                         };
                //     connection.Execute("ForumAppSchema.spUser_Upsert", userValues
                //                         , commandType: System.Data.CommandType.StoredProcedure);

                //     var topicValues = new { TopicName = "Topic"+i};
                //     connection.Execute("ForumAppSchema.spTopic_Upsert", topicValues
                //                         , commandType: System.Data.CommandType.StoredProcedure);
                // }

                for(int i=1; i<=1000; i++)
                {
                    var postValues = new {  UserId = rand.Next(1, 501),  
                                            Title = "Title_"+i,
                                            Content = "Post_"+i+" text content",
                                            FeaturedImage = "Post_"+i+".png"
                                        };
                    connection.Execute("ForumAppSchema.spPost_Upsert", postValues
                                        , commandType: System.Data.CommandType.StoredProcedure);

                    var senderId = rand.Next(1, 501);
                    var recipientId = rand.Next(1, 501);
                    while(recipientId == senderId)
                    {
                        recipientId = rand.Next(1, 501);
                    }

                    var friendRequestValues = new { SenderId = senderId, 
                                                    RecipientId = recipientId};
                    connection.Execute("ForumAppSchema.spFriendRequest_Insert", friendRequestValues
                                        , commandType: System.Data.CommandType.StoredProcedure);
                }

                for(int i=1; i<=500; i++)
                {
                    var commentValues = new {   UserId = rand.Next(1, 501), 
                                                PostId = rand.Next(1, 1001),
                                                Content = "Comment_"+i+" text content",
                                        };
                    connection.Execute("ForumAppSchema.spComment_Upsert", commentValues
                                        , commandType: System.Data.CommandType.StoredProcedure);
                }
            }
        }
    }
}