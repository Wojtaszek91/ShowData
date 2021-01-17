using Microsoft.EntityFrameworkCore;
using ShowData.Data;
using ShowData.Model;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository
{
    public class ProjectRepository : IProjectRepository
    {

            private readonly ApplicationDbContext _context;

            public ProjectRepository(ApplicationDbContext context)
            {
                _context = context;
            }
            public bool CreateProject(Project project)
            {
                _context.Projects.Add(project);
                return Save();
            }

            public bool DeleteProject(Project project)
            {
                _context.Projects.Remove(project);
                return Save();
            }

            public Project GetProject(int id)
            {
                return _context.Projects.FirstOrDefault(x => x.Id == id);
            }

            public ICollection<Project> GetProjectsList()
            {
                return _context.Projects.OrderBy(a => a.Title).ToList();
            }

            public bool IsProjectExists(string displayName)
            {
                return _context.Projects.Any(n => n.Title.ToLower().Trim() == displayName.ToLower().Trim());
            }
            public bool IsProjectExists(int id)
            {
                return _context.Projects.Any(n => n.Id == id);
            }

            public bool Save()
            {
                return _context.SaveChanges() >= 0 ? true : false;

            }

            public bool UpdateProject(Project project)
            {
                _context.Projects.Update(project);
                return Save();
            }
}
}
