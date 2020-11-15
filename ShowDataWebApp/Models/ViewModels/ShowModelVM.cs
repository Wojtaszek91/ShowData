using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models.ViewModels
{
    public class ShowModelVM
    {
        public IEnumerable<SelectListItem> DataOverviewList { get; set; }
        public ShowModel ShowModel { get; set; }
    }
}
