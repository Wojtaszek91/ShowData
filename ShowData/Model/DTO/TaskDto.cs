using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model.DTO
{
    public class taskDto
    {
        [Key]
        public int Id { get; set; }

        public string DisplayName { get; set; }
        public string TaskDescription { get; set; }

        public DateTime DisplayDate { get; set; }

        public bool isAvailsable { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public ProjectDto Project { get; set; }
    }
}
