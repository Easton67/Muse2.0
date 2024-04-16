using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Muse3.Models;

namespace Muse3.Controllers
{
    public class AdminController : Controller
    {
        // private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        // GET: Admin
        public ActionResult Index()
        {
            //return View(db.ApplicationUsers.ToList());
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View (userManager.Users.OrderBy(n => n.FamilyName).ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // ApplicationUser applicationUser = db.ApplicationUsers.Find(id);
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser applicationUser = userManager.FindById(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            // get a list of roles the user has and put them into a viewbag as roles
            // along with a list of roles the user doesn't have as noRoles
            var usrMgr = new LogicLayer.UserManager();
            var allRoles = usrMgr.SelectAllRoles();

            var roles = userManager.GetRoles(id);
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View(applicationUser);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult RemoveRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            if(role == "Admin")
            {
                var adminUsers = userManager.Users.ToList()
                    .Where(u => userManager.IsInRole(u.Id, "Admin"))
                    .ToList().Count();

                if(adminUsers < 2)
                {
                    ViewBag.Error = "Cannot remove the last Admin.";
                    return RedirectToAction("Details", "Admin", new { id = user.Id});
                }
            }

            userManager.RemoveFromRole(id, role);

            if(user.UserID != null)
            {
                try
                {
                    var _userManager = new LogicLayer.UserManager();
                    _userManager.DeleteUserRole((int)user.UserID, role);
                }
                catch (Exception)
                {
                    // nothing
                }
            }
            return RedirectToAction("Details", "Admin", new { id = user.Id });

            // get a list of roles the user has and put them into a viewbag as roles
            // along with a list of roles the user doesn't have as noRoles

            //var usrMgr = new LogicLayer.UserManager();
            //var allRoles = usrMgr.SelectAllRoles();

            //var roles = userManager.GetRoles(id);
            //var noRoles = allRoles.Except(roles);

            //ViewBag.Roles = roles;
            //ViewBag.NoRoles = noRoles;

            //return View("Details", user);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult AddRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.AddToRole(id, role);

            if(user.UserID != null)
            {
                try
                {
                    var _userManager = new LogicLayer.UserManager();
                    _userManager.AddUserRole((int)user.UserID, role);
                }
                catch (Exception)
                {

                    // do nothing
                }
            }
            return RedirectToAction("Details", "Admin", new { id = user.Id });



            // get a list of roles the user has and put them into a viewbag as roles
            // along with a list of roles the user doesn't have as noRoles

            //var usrMgr = new LogicLayer.UserManager();
            //var allRoles = usrMgr.SelectAllRoles();

            //var roles = userManager.GetRoles(id);
            //var noRoles = allRoles.Except(roles);

            //ViewBag.Roles = roles;
            //ViewBag.NoRoles = noRoles;

            //return View("Details", user);
        }
    }
}
