using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model.DTO
{
    public class ShowModelDto
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DateTime DisplayDate { get; set; }
        public bool isAvailsable { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public int DataOverviewId { get; set; }
        public DataOverviewDto DataOverview { get; set; }
    }
}
