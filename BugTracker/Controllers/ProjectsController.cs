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

            var id = item.ID;

            return RedirectToAction("Project", new { id });  //return view with blank project template **Placeholder**
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

        //FIX BUG
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
                //default
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
                    //Very bad idea, refactor later for switch statement
                    var model = new BugViewModel();
                    model.Project = project;
                    switch (categoryval)
                    {
                        case ("All"):
                            if (propertyval == "Description" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.Description).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.Description).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.Description).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.Description).ToList();
                            }
                            if (propertyval == "Description" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.Description).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.Description).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.Description).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.Description).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.Status).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.Status).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.Status).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.Status).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.Status).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.Status).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.Status).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.Status).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.DateModified).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.DateModified).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.DateModified).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.DateModified).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.DateModified).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.DateModified).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.DateCreated).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.DateCreated).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.DateCreated).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.DateCreated).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.DateCreated).ToList();
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.DateCreated).ToList();
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.DateCreated).ToList();
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.DateCreated).ToList();
                            }
                            model.categoryval = categoryval;
                            model.propertyval = propertyval;
                            model.orderval = orderval;
                            break;
                        case ("Bugs"):
                            if(propertyval == "Description" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.Description).ToList();
                            }
                            if (propertyval == "Description" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.Description).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.Status).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.Status).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Ascend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderBy(m => m.DateCreated).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Descend")
                            {
                                model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).OrderByDescending(m => m.DateCreated).ToList();
                            }
                            model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                            model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                            model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                            model.categoryval = categoryval;
                            model.propertyval = propertyval;
                            model.orderval = orderval;
                            break;
                        case ("In Progress"):
                            if (propertyval == "Description" && orderval == "Ascend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.Description).ToList();
                            }
                            if (propertyval == "Description" && orderval == "Descend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.Description).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Ascend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.Status).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Descend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.Status).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Ascend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Descend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Ascend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderBy(m => m.DateCreated).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Descend")
                            {
                                model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).OrderByDescending(m => m.DateCreated).ToList();
                            }
                            model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                            model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                            model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                            model.categoryval = categoryval;
                            model.propertyval = propertyval;
                            model.orderval = orderval;
                            break;
                        case ("Testing"):
                            if (propertyval == "Description" && orderval == "Ascend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.Description).ToList();
                            }
                            if (propertyval == "Description" && orderval == "Descend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.Description).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Ascend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.Status).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Descend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.Status).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Ascend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Descend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Ascend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderBy(m => m.DateCreated).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Descend")
                            {
                                model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).OrderByDescending(m => m.DateCreated).ToList();
                            }
                            model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                            model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                            model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).ToList();
                            model.categoryval = categoryval;
                            model.propertyval = propertyval;
                            model.orderval = orderval;
                            break;
                        case ("Completed"):
                            if (propertyval == "Description" && orderval == "Ascend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.Description).ToList();
                            }
                            if (propertyval == "Description" && orderval == "Descend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.Description).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Ascend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.Status).ToList();
                            }
                            if (propertyval == "Status" && orderval == "Descend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.Status).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Ascend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Modified" && orderval == "Descend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.DateModified).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Ascend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderBy(m => m.DateCreated).ToList();
                            }
                            if (propertyval == "Created" && orderval == "Descend")
                            {
                                model.Completed = project.GetBugs.Where(m => m.Category == Category.Complete).OrderByDescending(m => m.DateCreated).ToList();
                            }
                            model.Bug = project.GetBugs.Where(m => m.Category == Category.NoCategory).ToList();
                            model.InProgress = project.GetBugs.Where(m => m.Category == Category.InProgress).ToList();
                            model.Testing = project.GetBugs.Where(m => m.Category == Category.Testing).ToList();
                            model.categoryval = categoryval;
                            model.propertyval = propertyval;
                            model.orderval = orderval;
                            break;
                    }

                    return View(model);
                }
                
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
        public ActionResult BugDetails(int id, int bug)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var bugs = db.Projects.Find(id).GetBugs.Where(n => n.ID == bug).FirstOrDefault();

            return View("Project", bugs);    //placeholder (expand in view or new view)
        }

        // Edit Bug
        [HttpPost]
        public ActionResult BugEdit(int id, int bug, BugProperties model)
        {
            ModelState["Description"].Errors.Clear();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Project", new { id });
            }

            ApplicationDbContext db = new ApplicationDbContext();
            
            var bugs = db.Projects.Find(id).GetBugs.Where(n => n.ID == bug).FirstOrDefault();
            bugs.Category = model.Category;
            bugs.DateModified = DateTime.Now;
            bugs.ExpectedResult = model.ExpectedResult;
            bugs.RealityResult = model.RealityResult;
            bugs.Status = model.Status;
            bugs.OptionalInformation = model.OptionalInformation;

            db.Entry(bugs).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            

            return RedirectToAction("Project", new { id }); //placeholder
        }

        // Delete Bug
        [HttpPost]
        public ActionResult BugDelete(int id, int bug)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var toRemove = db.Projects.Find(id).GetBugs.Where(n => n.ID == bug).FirstOrDefault();
            if(toRemove != null)
            {
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Project", new { id }); //placeholder
            }
            else
                return RedirectToAction("Project", new { id });
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