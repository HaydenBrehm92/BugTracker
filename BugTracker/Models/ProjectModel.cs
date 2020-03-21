using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    //possibly useless
    public class CreateNewProject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This Field is Required")]
        [Range(1, 16, ErrorMessage = "Project name must be between 1 and 16 characters")]
        public string ProjectName { get; set; }

        public int ID { get; set; }
        public virtual string ApplicationUserID { get; set; }

    }

    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public virtual string ApplicationUserID { get; set; }
        public List<BugProperties> GetBugs { get; set; }
    }

    public class BugProperties
    {
        public enum Category {
            ToDo,
            InProgress,
            Complete
        }

        public enum Status { 
            Severe,
            Mild,
            Low
        }

        public string ID { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Category CategorySetGet { get; set; }

        [Required]
        public Status StatusSetGet { get; set; }
    }
}