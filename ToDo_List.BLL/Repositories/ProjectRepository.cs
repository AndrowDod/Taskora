using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ToDo_List.BLL.Interfaces;
using ToDO_List.DAL.Data;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.BLL.Repositories
{
    public class ProjectRepository : GenericRepository<Project> ,IProjectRepository
    {
        private readonly ToDoDbContext _context;

        public ProjectRepository(ToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<Project> GetAll()
        {
            return _context.Set<Project>()
                .Include(p => p.Tasks)
                .ToList();
        }

        public override Project GetById(int id)
        {
            return _context.Set<Project>()
                .Where(p => p.Id == id)
                .Include(p => p.Tasks).FirstOrDefault();
        }

        public  IEnumerable<Project> GetByName(string name , string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _context.Projects
                    .Include(p => p.Tasks)
                    .Where(p => p.userId != null && p.userId == userId)
                    .ToList();


            return _context.Projects
                    .Where(p => p.Name.ToLower().Contains(name.ToLower())
                             && p.userId != null
                             && p.userId == userId)
                    .Include(p => p.Tasks)
                    .ToList();

        }



    }

}
