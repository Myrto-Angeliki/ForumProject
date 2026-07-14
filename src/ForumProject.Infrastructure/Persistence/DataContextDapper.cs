using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ForumProject.Infrastructure.Persistence
{
    public class DataContextDapper
    {
        private readonly string _connectionString;
        public DataContextDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public IEnumerable<T> LoadData<T>(string sql)
        {
            return CreateConnection().Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            return CreateConnection().QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            return CreateConnection().Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            return CreateConnection().Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, DynamicParameters parameters)
        {
            return CreateConnection().Execute(sql, parameters) > 0;
        }

        public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters)
        {
            return CreateConnection().Query<T>(sql, parameters);
        }

        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
        {
            return CreateConnection().QuerySingle<T>(sql, parameters);
        }
    }
}