using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

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

            db.Projects.Add(item);
            db.SaveChanges();

            return RedirectToAction("Index");  //return view with blank project template
        }

        // Get
        // Populate dropdown with project names
        // May need to add recent timestamps
        //public List<Project> OpenProjectDropdown()
        //{
        //    var list = new List<Project>();
        //    ApplicationDbContext db = new ApplicationDbContext();

        //    list = db.Projects.Where(n => n.ApplicationUserID == User.Identity.GetUserId()).ToList();

        //    return list;
        //}


        // Get
        // Home/_OpenProject
        // Possibly need to pass parameter into OpenProject to grab correct Project
        // Need to figure out if i should make a seperate function for populating the dropdown field*
        public ActionResult OpenProject()
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //Project project = db.Projects.Find(User.Identity.GetUserId());

            //return RedirectToAction("Index");

            
        //{
            List<Project> list = new List<Project>();
            ApplicationDbContext db = new ApplicationDbContext();
            string currentId = User.Identity.GetUserId();
            list = db.Projects.Where(n => n.ApplicationUserID == currentId).ToList();

            return View(list);
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