using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public int TasksIncluded { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime DeadLine { get; set; }
        public ICollection<task> task { get; set; }
    }
}
