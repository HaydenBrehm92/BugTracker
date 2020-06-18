using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace BugTracker.Controllers
{
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
            int pageSize = 5;
            int pageNumber = (page ?? 1); //need to change possible for initial page create
            var list = new ProjectViewModel()
            {
                RecentlyUsed = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateModified).Take(3).ToList(),
                All = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateCreated).ToPagedList(pageNumber,pageSize),
                
            };

            //if (list.All != null)
            //{
            //    page = 1;
            //}

            return View(list);

            //List<Project> list = new List<Project>();
            //ApplicationDbContext db = new ApplicationDbContext();
            //string currentId = User.Identity.GetUserId();
            //list = db.Projects.Where(n => n.ApplicationUserID == currentId).ToList();

            //return View(list);
        }

        public ActionResult UpdateListPage(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentId = User.Identity.GetUserId();
            var dataModel = new ProjectViewModel()
            {
                All = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateCreated).ToPagedList(page ?? 1, 5),
            };

            return PartialView("_OpenProjectPartial", dataModel);
        }


        //public ActionResult _OpenProjectPartial(int? page)
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    string currentId = User.Identity.GetUserId();
        //    int pageSize = 5;
        //    int pageNumber = (page ?? 1);
        //    var list = new ProjectViewModel()
        //    {
        //        All = db.Projects.Where(n => n.ApplicationUserID == currentId).OrderByDescending(t => t.DateCreated).ToPagedList(pageNumber, pageSize)
        //    };
        //    return PartialView(list);
        //}

        //Get
        //Projects/Project#
        public ActionResult Project(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var project = db.Projects.Where(n => n.ID == id);
            return View(project);
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