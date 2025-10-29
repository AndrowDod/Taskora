using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToDo_List.BLL.Interfaces;
using ToDO_List.DAL.Data;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.BLL.Repositories
{
    public class TasksRepository : GenericRepository<Tasks> , ITasksRepository
    {
        private readonly ToDoDbContext _context;

        public TasksRepository(ToDoDbContext context) : base(context)
        {
           _context = context;
        }
        public override IEnumerable<Tasks> GetAll()
        {
            return _context.Set<Tasks>()
                .Include(t => t.Project)
                .ToList();
        }

        public IEnumerable GetByTitle(string taskTitle , string userId)
        {
            if (string.IsNullOrWhiteSpace(taskTitle))
                return _context.Tasks.
                    Include(t => t.Project)
                    .Where(t => t.userId != null && t.userId == userId)
                    .ToList();

            return _context.Tasks
                .Where(t => t.Title.Contains(taskTitle) 
                        && t.userId != null
                        && t.userId == userId)
                .Include(t => t.Project)
                .ToList();
        }
    }
}
