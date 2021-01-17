using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model
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

        public byte[] Image { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
