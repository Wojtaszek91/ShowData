using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model.DTO
{
    public class ShowModelUpsterDto
    {
        [Required]
        public string DisplayName { get; set; }
        public DateTime DisplayDate { get; set; }
        public bool isAvailsable { get; set; }
        [Required]
        public int DataOverviewId { get; set; }
    }
}
