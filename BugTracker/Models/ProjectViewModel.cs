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
        //public List<Project> RecentlyUsed { get; set; }
        //public List<Project> All { get; set; }
        public IPagedList<Project> PageData { get; set; }
    }
}