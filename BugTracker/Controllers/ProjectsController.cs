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
                return View(model); //This needs to return the view with the partial view loaded
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
        // Home/_OpenProject
        public ActionResult OpenProject()
        {
            return PartialView("~/Views/Projects/_OpenProject.cshtml");
        }
    }
}