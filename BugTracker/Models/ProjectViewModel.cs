using System;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectViewModel
    {
        public List<Project> RecentlyUsed { get; set; }
        public IPagedList<Project> All { get; set; }
        
    }
}