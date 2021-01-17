using ShowData.Model;
using ShowData.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository.IRepository
{

        public interface IProjectRepository
        {
            ICollection<Project> GetProjectsList();

            Project GetProject(int id);

            bool IsProjectExists(string title);

            bool IsProjectExists(int id);

            bool CreateProject(Project project);

            bool UpdateProject(Project project);

            bool DeleteProject(Project project);

            bool Save();

        }
}


