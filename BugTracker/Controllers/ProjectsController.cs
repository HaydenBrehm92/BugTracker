using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    public static class Extensions
    {
        // SORT BY CATEGORY
        public static IQueryable<BugProperties> Sort(this IQueryable<BugProperties> source, Category category, string property, string order)
        {
            return source.Where("Category = @0", category).OrderBy(property + (order == "Descend" ? " descending" : ""));
        }

        // SORT ALL CARDS IN PROJECT
        public static IQueryable<BugProperties> SortAll(this IQueryable<BugProperties> source, string property, string order)
        {
            return source.OrderBy(property + (order == "Descend" ? " descending" : ""));
        }
    }

    [Authorize]
    public class ProjectsController : Controller
    {
        //GET: Projects
        public ActionResult Index()
        {
            return View();
        }

        //Post
        // Home/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(CreateNewProject model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationDbContext db = new ApplicationDbContext();

            var item = new Project();
            item.GetBugs = new List<BugProperties>();
            item.ProjectName = model.ProjectName;
            item.ApplicationUserID = User.Identity.GetUserId();
            item.DateCreated = DateTime.UtcNow;
            item.DateModified = DateTime.UtcNow;

            db.Projects.Add(item);
            db.SaveChanges();

            var id = item.ID;

            return RedirectToAction("Project", new { id });
        }

        //order by most recently modified
        // Get
        // Home/_OpenProject
        public ActionResult OpenProject(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentId = User.Identity.GetUserId();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var model = new ProjectViewModel()
            {
                Page = page,
                PageData = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateModified).ToPagedList(pageNumber, pageSize)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Project toRemove = db.Projects.Find(id);
            if (toRemove != null)
            {
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("OpenProject");
            }
            else
                return RedirectToAction("OpenProject"); //prevents backbutton from seeing deleted entry
        }

        //Get
        //Projects/Project#
        public ActionResult Project(int id, string categoryval = "", string propertyval = "", string orderval = "")
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Project project = db.Projects.Find(id);
            if(project == null)
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            else
            {
                // DEFAULT
                if(categoryval == "" || propertyval == "" || orderval == "")
                {
                    var model = new BugViewModel()
                    {
                        Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList(),
                        InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList(),
                        Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList(),
                        Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList(),
                        Project = project,
                        categoryval = categoryval,
                        propertyval = propertyval,
                        orderval = orderval
                    };
                    return View(model);
                }
                else
                {
                    // SORT BASED ON USER INPUT FIELDS
                    var model = new BugViewModel() { Project = project};
                    if (categoryval != "All")
                    {
                        Category currentCategory = 0;
                        // Not the best implementation but much better than it used to be.
                        switch (categoryval)
                        {
                            case "Bugs":
                                currentCategory = Category.NoCategory;
                                // BUGVIEWMODEL
                                model.Bug = project.GetBugs.AsQueryable().Sort(currentCategory, propertyval, orderval).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                                break;
                            case "In Progress":
                                currentCategory = Category.InProgress;
                                // BUGVIEWMODEL
                                model.InProgress = project.GetBugs.AsQueryable().Sort(currentCategory, propertyval, orderval).ToList();
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                                break;
                            case "Testing":
                                currentCategory = Category.Testing;
                                // BUGVIEWMODEL
                                model.Testing = project.GetBugs.AsQueryable().Sort(currentCategory, propertyval, orderval).ToList();
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                                break;
                            case "Completed":
                                currentCategory = Category.Complete;
                                // BUGVIEWMODEL
                                model.Completed = project.GetBugs.AsQueryable().Sort(currentCategory, propertyval, orderval).ToList();
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                                break;
                        };
                    }
                    else
                    {
                        model.Project.GetBugs = project.GetBugs.AsQueryable().SortAll(propertyval, orderval).ToList();

                        // BUGVIEWMODEL 
                        model.Bug = model.Project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                        model.InProgress = model.Project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                        model.Testing = model.Project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                        model.Completed = model.Project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                    }

                    model.categoryval = categoryval;
                    model.propertyval = propertyval;
                    model.orderval = orderval;

                    return View(model);
                }     
            }
        }

        //Create Bug
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBug(int id, BugProperties model, string categoryval = "", string propertyval = "", string orderval = "")
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Project", new { id, categoryval, propertyval, orderval }); 
            }
            
            ApplicationDbContext db = new ApplicationDbContext();
            Project project = db.Projects.Find(id);
            BugProperties bug = new BugProperties
            {
                //Category = model.Category,
                Category = 0,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Description = model.Description,
                Status = model.Status,
                ExpectedResult = model.ExpectedResult,
                RealityResult = model.RealityResult,
                OptionalInformation = model.OptionalInformation
            };
            //project.GetBugs.Add(bug);
            if (project.GetBugs == null)
            {
                project.GetBugs = new List<BugProperties>
                {
                    bug
                };

            }
            else
                project.GetBugs.Add(bug);

            project.DateModified = DateTime.UtcNow; //project has been modified
            db.SaveChanges();

            return RedirectToAction("Project", new { id, categoryval, propertyval, orderval });
        }

        // Edit Bug
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BugEdit(int id, int bug, BugProperties model, string categoryval = "", string propertyval = "", string orderval = "")
        {
            ModelState["Description"].Errors.Clear();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Project", new { id, categoryval, propertyval, orderval });
            }

            ApplicationDbContext db = new ApplicationDbContext();

            Project project = db.Projects.Find(id);
            var bugs = db.Projects.Find(id).GetBugs.Where(n => n.ID == bug).FirstOrDefault();
            bugs.Category = model.Category;
            bugs.DateModified = DateTime.UtcNow;
            bugs.ExpectedResult = model.ExpectedResult;
            bugs.RealityResult = model.RealityResult;
            bugs.Status = model.Status;
            bugs.OptionalInformation = model.OptionalInformation;

            db.Entry(bugs).State = System.Data.Entity.EntityState.Modified; //Possiblly not needed because Entity Framework does it automatically. Could just modify then save
            project.DateModified = DateTime.UtcNow;
            db.SaveChanges();

            return RedirectToAction("Project", new { id, categoryval, propertyval, orderval }); 
        }

        // Delete Bug
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BugDelete(int id, int bug, string categoryval = "", string propertyval = "", string orderval = "")
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Project project = db.Projects.Find(id);
            var toRemove = db.Projects.Find(id).GetBugs.Where(n => n.ID == bug).FirstOrDefault();
            if(toRemove != null)
            {
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                project.DateModified = DateTime.UtcNow;
                db.SaveChanges();
                return RedirectToAction("Project", new { id, categoryval, propertyval, orderval }); //placeholder
            }
            else
                return RedirectToAction("Project", new { id, categoryval, propertyval, orderval });
        }

        // Checks for dup Project Names
        [HttpPost]
        public JsonResult CheckName(string ProjectName)
        {
            string id = User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();
            return Json(!db.Projects.Any(s => s.ProjectName == ProjectName && s.ApplicationUserID == id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OpenProjectConvertUtcToLocal(string timezone)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            
            var dates = new List<object>();
            string id = User.Identity.GetUserId();
            var projectsList = context.Projects.Where(s => s.ApplicationUserID == id).Select(d => new {id = d.ID, created = d.DateCreated, modified = d.DateModified}).OrderByDescending(t => t.modified).ToList(); //grab all projects' dates
            timezone = TimeZoneInfo.Local.ToSerializedString();
            TimeZoneInfo zoneInfo = TimeZoneInfo.FromSerializedString(timezone);

            foreach (var project in projectsList)
            {
                var created = TimeZoneInfo.ConvertTimeFromUtc(project.created, zoneInfo);   //convert date created
                var modified = TimeZoneInfo.ConvertTimeFromUtc(project.modified, zoneInfo); //convert date modified
                dates.Add(new { project.id, CreatedDate = created, ModifiedDate = modified });
            }
            
            return Json(dates, "application/json");
        }

        [HttpPost]
        public JsonResult BugConvertUtcToLocal(string timezone, int projectid)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var dates = new List<object>();
            string id = User.Identity.GetUserId();
            if(!context.Projects.Any(s => s.ApplicationUserID == id && s.ID == projectid))
            {
                Response.StatusCode = 404;
                return Json("Error");
            }
            var bugList = context.Projects.Find(projectid).GetBugs.Select(b => new { id = b.ID, created = b.DateCreated, modified = b.DateModified }).ToList(); //grab all bugs' dates
            timezone = TimeZoneInfo.Local.ToSerializedString();
            TimeZoneInfo zoneInfo = TimeZoneInfo.FromSerializedString(timezone);

            foreach (var project in bugList)
            {
                var created = TimeZoneInfo.ConvertTimeFromUtc(project.created, zoneInfo);   //convert date created
                var modified = TimeZoneInfo.ConvertTimeFromUtc(project.modified, zoneInfo); //convert date modified
                dates.Add(new { project.id, CreatedDate = created, ModifiedDate = modified });
            }

            return Json(dates, "application/json");
        }
    } 
}