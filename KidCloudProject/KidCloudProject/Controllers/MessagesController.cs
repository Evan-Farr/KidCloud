using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Chat.V2.Service.Channel;
using KidCloudProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KidCloudProject.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        const string accountSid = "AC383fc67c15e32582075da33d541fd388";
        const string authToken = "7915107c0ef796ad3d5a51fe2cdd0552";
        const string serviceSid = "IS9acc1cbb2d874347a95cacebfc5cd5aa";

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            TwilioClient.Init(accountSid, authToken);

            string channelSid = GetChannelId();

            if (channelSid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = User.Identity.Name;
            return View(MessageResource.Read(serviceSid, channelSid));
            //return View();
        }

        private string GetChannelId()
        {
            string userId = User.Identity.GetUserId();

            if (isUser("Admin"))
            {
                //channelSid = db.Parents.Where(p => p.UserId.Id == userId).First().DayCareId.ChannelId;
                return null;
            }
            else if (isUser("DayCare"))
            {
                ViewBag.DayCareName = db.DayCares.Where(e => e.UserId.Id == userId).First().Name;
                return db.DayCares.Where(e => e.UserId.Id == userId).First().ChannelId;
            }
            else if (isUser("Employee"))
            {
                ViewBag.DayCareName = db.Employees.Where(e => e.UserId.Id == userId).First().DayCareId.Name;
                return db.Employees.Where(e => e.UserId.Id == userId).First().DayCareId.ChannelId;
            }
            else if (isUser("Parent"))
            {
                ViewBag.DayCareName = db.Parents.Where(e => e.UserId.Id == userId).First().DayCare.Name;
                return db.Parents.Where(p => p.UserId.Id == userId).First().DayCare.ChannelId;
            }

            return null;
        }

        private bool isUser(string role)
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