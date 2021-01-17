using ShowDataWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Repository.IRepository
{
    public interface ItaskRepository : IRepository<task>
    {
        Task<IEnumerable<task>> GetAllProjectTasks(string url, string id, bool isProject, string token);
    }
}
