using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Model.DTO
{
    public class ShowModelDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DateTime DisplayDate { get; set; }
        public bool isAvailsable { get; set; }
    }
}
