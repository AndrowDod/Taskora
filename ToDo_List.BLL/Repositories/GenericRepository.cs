using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_List.BLL.Interfaces;
using ToDO_List.DAL.Data;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ToDoDbContext _context;
        public GenericRepository(ToDoDbContext context) 
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();

        }
    }
}
