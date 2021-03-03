using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<BugProperties> GetBugs { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

    public enum Category
    {
        [Display(Name ="Unassigned")]
        NoCategory,
        [Display(Name ="In Progress")]
        InProgress,
        [Display(Name = "Testing")]
        Testing,
        [Display(Name = "Completed")]
        Complete
    }

    public enum Status
    {
        Trivial,
        Low,
        Moderate,
        Severe
    }

    public class BugProperties
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This Field is Required")]
        [StringLength(140, ErrorMessage = "Description must be between 1 and 140 characters")]
        [Display(Name = "Description*")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Bug Severity")]
        public Status Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This Field is Required")]
        [StringLength(140, ErrorMessage = "Expected result must be between 1 and 140 characters")]
        [Display(Name = "Expected Result*")]
        public string ExpectedResult { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This Field is Required")]
        [StringLength(140, ErrorMessage = "Real result must be between 1 and 140 characters")]
        [Display(Name = "Real Result*")]
        public string RealityResult { get; set; }

        [StringLength(140, ErrorMessage = "Optional information must be between 1 and 140 characters")]
        [Display(Name = "Optional Information")]
        public string OptionalInformation { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public Project Project { get; set; }
        
    }

    public static class CategoryExtensions
    {
        public static string ToDisplay(this Enum val)
        {
            return val.GetType()
                .GetMember(val.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
    }



}