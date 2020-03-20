using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class CreateNewProject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage ="This Field is Required")]
        [Range(1,16,ErrorMessage = "Project name must be between 1 and 16 characters")]
        public string ProjectName { get; set; }


    }
}