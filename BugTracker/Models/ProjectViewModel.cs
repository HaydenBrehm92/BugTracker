using System;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class ProjectViewModel
    {
        public int? Page { get; set; }
        public IPagedList<Project> PageData { get; set; }
    }

    public class BugViewModel
    {
        public Project Project { get; set; }
        public List<BugProperties> Bug { get; set; }
        public List<BugProperties> InProgress { get; set; }
        public List<BugProperties> Testing { get; set; }
        public List<BugProperties> Completed { get; set; }

        public string categoryval { get; set; }
        public string propertyval { get; set; }
        public string orderval { get; set; }
    }

    
}