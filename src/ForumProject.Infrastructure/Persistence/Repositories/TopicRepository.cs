using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Domain.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DataContextDapper _context;

        public TopicRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Topic topic)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spTopic_Upsert"
                                            , new {TopicName = topic.TopicName}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int topicId)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spTopic_Delete"
                                            , new {TopicId = topicId}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Topic> topics = await connection.QueryAsync<Topic>(
                                            "ForumAppSchema.spTopic_Get"
                                            , commandType: CommandType.StoredProcedure);
                return topics ;
            }
        }

        public async Task<Topic?> GetByIdAsync(int topicId)
        {
            using(var connection = _context.CreateConnection())
            {
                Topic? topic = await connection.QuerySingleAsync<Topic>(
                                            "ForumAppSchema.spTopic_Get"
                                            , new {TopicId = topicId}
                                            , commandType: CommandType.StoredProcedure);
                return topic;
            }
        }

        public async Task<IEnumerable<Topic>> GetByPostAsync(int postId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Topic> topic = await connection.QueryAsync<Topic>(
                                            "ForumAppSchema.spPost_GetTopics"
                                            , new {PostId = postId}
                                            , commandType: CommandType.StoredProcedure);
                return topic;
            }
        }

        public async Task<IEnumerable<Topic>> GetByUserAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<Topic> topic = await connection.QueryAsync<Topic>(
                                            "ForumAppSchema.spUser_GetTopics"
                                            , new {UserId = userId}
                                            , commandType: CommandType.StoredProcedure);
                return topic;
            }
        }

        public async Task<bool> UpdateAsync(Topic topic)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spTopic_Upsert"
                                            , new {TopicId = topic.TopicId, TopicName = topic.TopicName}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}