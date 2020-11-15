using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model.DTO
{
    public class DataOverviewCreateDto
    {
            [Required]
            public int dataIncluded { get; set; }
            [Required]
            public string Title { get; set; }
            public byte[] Image { get; set; }
            public DateTime SomeDate { get; set; }
    }
}
