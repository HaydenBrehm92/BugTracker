using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace BugTracker.Models
{
    public class CreateNewProject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This Field is Required")]
        [StringLength(16, ErrorMessage = "Project name must be between 1 and 16 characters")]
        [Remote("CheckName", "Projects", HttpMethod = "POST", ErrorMessage = "Project Name Already Exists")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
    }

    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public virtual string ApplicationUserID { get; set; }
        public List<BugProperties> GetBugs { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

    public enum Category
    {
        ToDo,
        InProgress,
        Complete
    }

    public enum Status
    {
        Severe,
        Mild,
        Low
    }

    public class BugProperties
    {
        public string ID { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }

    
}