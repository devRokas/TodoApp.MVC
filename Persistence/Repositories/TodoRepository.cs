using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private const string TableName = "TodoItems";
        private readonly ISqlClient _sqlClient;

        public TodoRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }
        
        public Task<IEnumerable<TodoModel>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";

            return _sqlClient.QueryAsync<TodoModel>(sql);
        }

        public Task<TodoModel> Get(Guid? id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";

            return _sqlClient.QuerySingleOrDefaultAsync<TodoModel>(sql, new { Id = id });
        }

        public Task<int> SaveOrUpdate(TodoModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, Title, Description, DateCreated) 
                        VALUES (@Id, @Title, @Description, @DateCreated)
                        ON DUPLICATE KEY UPDATE Title = @Title, Description = @Description";

            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<int> Delete(Guid? id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id";
            
            return _sqlClient.ExecuteAsync(sql, new { Id = id });
        }
    }
}