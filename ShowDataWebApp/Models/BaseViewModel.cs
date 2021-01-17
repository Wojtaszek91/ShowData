using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Models
{
    public class BaseViewModel
    {
        public string MainDiv { get; } = "jumbotron";
        public string AlertDiv { get; } = "alert alert-dismissible alert-danger";
        public string FormGroup { get; } = "form-group";
        public string SubmitButton { get; } = "btn btn-dark";
    }
}
