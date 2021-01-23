using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models
{
    public class task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string TaskDescription { get; set; }

        public DateTime DisplayDate { get; set; }

        public bool isAvailsable { get; set; }

        [Required]
        public string UserForTask { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
