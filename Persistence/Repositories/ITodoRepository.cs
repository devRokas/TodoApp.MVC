using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoModel>> GetAllAsync();

        Task<TodoModel> Get(Guid? id);

        Task<int> SaveOrUpdate(TodoModel model);

        Task<int> Delete(Guid? id);
    }
}