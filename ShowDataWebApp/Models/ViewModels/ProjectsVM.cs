using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models.ViewModels
{
    public class ProjectsVM
    {
        public IEnumerable<Project> ProjectsList { get; set; }
    }
}
