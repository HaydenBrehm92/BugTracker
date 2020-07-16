using System;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using System.Web;

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
    }
}