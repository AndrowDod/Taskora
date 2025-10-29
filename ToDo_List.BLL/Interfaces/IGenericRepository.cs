using System.Collections.Generic;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : ModelBase
    {
            IEnumerable<T> GetAll();
            T GetById(int id);
            void Create(T entity);
            void Update(T entity);
            void Delete(T entity);

    }
}
