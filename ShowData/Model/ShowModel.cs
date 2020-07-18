using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model
{
    public class ShowModel
    {
        [Key]
        public int ShowModelId { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public DateTime DisplayDate { get; set; }
        public bool isAvailsable { get; set; }
        [Required]
        public int DataOverviewId { get; set; }
        [ForeignKey("DataOverviewId")]
        public DataOverview DataOverview { get; set; }
    }
}
