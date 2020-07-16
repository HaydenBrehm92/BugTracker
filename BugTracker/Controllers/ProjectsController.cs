using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        //GET: Projects
        public ActionResult Index()
        {
            return View();
        }

        // Get
        // Home/CreateProject
        public ActionResult CreateProject()
        {
            return PartialView("~/Views/Projects/CreateProject.cshtml");
        }

        //Post
        // Home/CreateProject
        [HttpPost]
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
            item.DateCreated = DateTime.UtcNow; //Will NOT be showing this
            item.DateModified = DateTime.Now; //I want to show this

            db.Projects.Add(item);
            db.SaveChanges();

            return RedirectToAction("Index");  //return view with blank project template **Placeholder**
        }

        // Get
        // Home/_OpenProject
        public ActionResult OpenProject(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentId = User.Identity.GetUserId();
            int pageSize = 10;
            int pageNumber = (page ?? 1); //need to change possible for initial page create
            var model = new ProjectViewModel()
            {
                Page = page,
                PageData = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateCreated).ToPagedList(pageNumber, pageSize)
            };
            return View(model);
        }

        //public PartialViewResult _OpenProjectPartial(ProjectViewModel model)
        //{
        //    return PartialView(model);
            
        //}

        //DELETE METHOD GOES HERE!!!!!!
        [HttpPost]
        public ActionResult DeleteProject(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Project toRemove = db.Projects.Find(id);
            if (toRemove != null)
            {
                db.Projects.Remove(toRemove);
                db.SaveChanges();
                return RedirectToAction("OpenProject");
            }
            else
                return RedirectToAction("OpenProject"); //prevents backbutton from seeing deleted entry
        }

        //Get
        //Projects/Project#
        public ActionResult Project(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Project project = db.Projects.Find(id);
            if(project == null)
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            else
            {

                var model = new BugViewModel()
                {
                    Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList(),
                    InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList(),
                    Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList(),
                    Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList(),
                    Project = project
                };
                return View(model);
            }

        }

        //Create Bug
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBug(int id, BugProperties model)
        {
            if (!ModelState.IsValid)
            {
                return View("Project", model);
            }
            
            ApplicationDbContext db = new ApplicationDbContext();
            Project project = db.Projects.Find(id);
            BugProperties bug = new BugProperties
            {
                //Category = model.Category,
                Category = 0,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.Now,
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

            db.SaveChanges();

            return RedirectToAction("Project", new { id });
        }

        // Read Bug
        public ActionResult BugDetails(int id, int idbug)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var bug = db.Projects.Find(id).GetBugs.Where(n => n.ID == idbug).FirstOrDefault();

            return View("Project", bug);    //placeholder (expand in view or new view)
        }

        // Edit Bug
        [HttpPost]
        public ActionResult BugEdit(int id, int idbug, BugProperties model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            var bug = db.Projects.Find(id).GetBugs.Where(n => n.ID == idbug).FirstOrDefault();
            bug.Category = model.Category;
            bug.DateModified = DateTime.Now;
            bug.Description = model.Description;
            bug.ExpectedResult = model.ExpectedResult;
            bug.RealityResult = model.RealityResult;
            bug.Status = model.Status;
            bug.OptionalInformation = model.OptionalInformation;

            db.Entry(bug).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            

            return RedirectToAction("Project"); //placeholder
        }

        // Delete Bug
        [HttpPost]
        public ActionResult BugDelete(int id, int idbug)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var toRemove = db.Projects.Find(id).GetBugs.Where(n => n.ID == idbug).FirstOrDefault();
            if(toRemove != null)
            {
                db.Projects.Find(id).GetBugs.Remove(toRemove);
                db.SaveChanges();
                return RedirectToAction("Project"); //placeholder
            }
            else
                return RedirectToAction("Project");
        }

        // Checks for dup Project Names
        [HttpPost]
        public JsonResult CheckName(string ProjectName)
        {
            string id = User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();
            return Json(!db.Projects.Any(s => s.ProjectName == ProjectName && s.ApplicationUserID == id), JsonRequestBehavior.AllowGet);
        }
    }
}