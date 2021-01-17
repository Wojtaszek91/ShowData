using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models.ViewModels
{
    public class TasksViewVM
    {
        public IEnumerable<task> TasksList { get; set; }
        public Project Project { get; set; }
    }
}
