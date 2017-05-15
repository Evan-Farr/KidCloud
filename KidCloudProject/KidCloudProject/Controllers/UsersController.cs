using KidCloudProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TrashPickup.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";
                ApplicationDbContext db = new ApplicationDbContext();
                if (isUser("Admin"))
                {
                    ViewBag.displayMenu = "Yes";
                }
                else if (isUser("Parent"))
                {
                    string holder = user.GetUserId();
                    var temp = db.Parents.Where(i => i.UserId.Id == holder).FirstOrDefault().Id;
                    var parent = db.Parents.Where(b => b.UserId.Id == holder).Select(q => q).FirstOrDefault();
                    ViewBag.Id = temp;
                    List<SelectListItem> kids = new List<SelectListItem>();
                    foreach (var child in parent.Children)
                    {
                        kids.Add(new SelectListItem { Text = child.FirstName + " " + child.LastName, Value = child.Id.ToString() });
                    }
                    ViewBag.Children = kids;
                    ViewBag.displayMenu = "Parent";
                }
                else if (isUser("Employee"))
                {
                    string holder = user.GetUserId();
                    var temp = db.Employees.Where(i => i.UserId.Id == holder).FirstOrDefault().Id;
                    ViewBag.Id = temp;
                    ViewBag.displayMenu = "Employee";
                }
                else if (isUser("DayCare"))
                {
                    string holder = user.GetUserId();
                    var temp = db.DayCares.Where(l => l.UserId.Id == holder).FirstOrDefault().Id;
                    ViewBag.Id = temp;
                    var temp1 = db.DayCares.Where(i => i.UserId.Id == holder).FirstOrDefault().ZipCode;
                    var dayCare = db.DayCares.Where(t => t.UserId.Id == holder).Select(s => s).FirstOrDefault();
                    ViewBag.ZipCode = temp1;
                    List<SelectListItem> children = new List<SelectListItem>();
                    foreach(var child in dayCare.Children)
                    {
                        children.Add(new SelectListItem { Text = child.FirstName + " " + child.LastName, Value = child.Id.ToString() });
                    }
                    ViewBag.Children = children;
                    ViewBag.displayMenu = "DayCare";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
            return View();
        }

        public bool isUser(string role)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == role)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}