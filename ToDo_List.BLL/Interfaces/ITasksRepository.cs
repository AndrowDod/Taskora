using System.Collections;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.BLL.Interfaces
{
    public interface ITasksRepository : IGenericRepository<Tasks>
    {
        IEnumerable GetByTitle(string taskTitle,string userId);

    }
}
