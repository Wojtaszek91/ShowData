using ShowData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository.IRepository
{
    public interface ItaskRepository
    {
        ICollection<task> GettaskList();

        ICollection<task> GetTasksFromSpecificProject(int projectId);

        task Gettask(int id);

        bool IstaskExists(string id);

        bool IstaskExists(int id);

        bool Createtask(task task);

        bool Updatetask(task task);

        bool Deletetask(task task);

        bool Save();
            
    }
}
