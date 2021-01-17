using Microsoft.EntityFrameworkCore;
using ShowData.Data;
using ShowData.Model;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowData.Repository
{
    public class taskRepository : ItaskRepository
    {
        private readonly ApplicationDbContext _context;

        public taskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Createtask(task task)
        {
            _context.tasks.Add(task);
            return Save();
        }

        public bool Deletetask(task task)
        {
            _context.tasks.Remove(task);
            return Save();
        }

        public task Gettask(int id)
        {
          return  _context.tasks.Include(d => d.Project).FirstOrDefault(x => x.Id == id);
        }

        public ICollection<task> GetTasksFromSpecificProject(int dataOverviewId)
        {
            return _context.tasks.Include(d => d.Project).Where(d => d.ProjectId == dataOverviewId).ToList();
        }
        public ICollection<task> GettaskList()
        {
            return _context.tasks.Include(d => d.Project).OrderBy(a => a.DisplayName).ToList();
        }

        public bool IstaskExists(string displayName)
        {
          return  _context.tasks.Any(n => n.DisplayName.ToLower().Trim() == displayName.ToLower().Trim());
        }
        public bool IstaskExists(int id)
        {
          return  _context.tasks.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
            
        }

        public bool Updatetask(task task)
        {
            _context.tasks.Update(task);
            return Save();

        }
    }
}
