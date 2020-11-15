using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models
{
    public class DataOverview
    {
        public int Id { get; set; }
        [Required]
        public int dataIncluded { get; set; }
        [Required]
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public DateTime SomeDate { get; set; }
        public ICollection<ShowModel> showModel { get; set; }
    }
}
