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
using KidCloudProject._APIs;
using Twilio.Rest.Chat.V2.Service;

namespace KidCloudProject.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DirectMessage(string Receiver)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser receiver = db.Users.Where(u => u.UserName == Receiver).First();
                ApplicationUser sender = db.Users.Where(u => u.Id == userId).First();

                ViewBag.UserName = sender.UserName;
                ViewBag.Receiver = receiver.UserName;
                DirectMessageChannel dmChannel = null;

                // API Connect
                TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);

                try
                {
                    dmChannel = db.DirectMessageChannels.Where(dm => dm.ReciverId.Id == receiver.Id && dm.SenderId.Id == sender.Id || dm.ReciverId.Id == sender.Id && dm.SenderId.Id == receiver.Id).First();
                    return View(MessageResource.Read(TwilioApiKeys.serviceSid, dmChannel.ChannelId));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


                if (dmChannel == null)
                {
                    // API Create channel
                    var channel = ChannelResource.Create(TwilioApiKeys.serviceSid, friendlyName: "DM Channel", uniqueName: $"{sender.UserName}-{receiver.UserName}", type: ChannelResource.ChannelTypeEnum.Private);

                    // Database DMChannel
                    dmChannel = new DirectMessageChannel();
                    dmChannel.ChannelId = channel.Sid;
                    dmChannel.ReciverId = receiver;
                    dmChannel.SenderId = sender;

                    // API Add members
                    MemberResource.Create(TwilioApiKeys.serviceSid, channel.Sid, dmChannel.ReciverId.UserName);
                    MemberResource.Create(TwilioApiKeys.serviceSid, channel.Sid, dmChannel.SenderId.UserName);

                    // Save DMChannel to Database
                    db.DirectMessageChannels.Add(dmChannel);
                    db.SaveChanges();

                    return View(MessageResource.Read(TwilioApiKeys.serviceSid, channel.Sid));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View("Index");
        }

        public ActionResult ViewDirectMessageChannels()
        {
            string userId = User.Identity.GetUserId();

            DayCare daycare = null;
            ApplicationUser user = null;

            if (isUser("Admin"))
            {
                return Content("Admin stop!");
            }
            else if (isUser("DayCare"))
            {
                daycare = db.DayCares.Where(d => d.UserId.Id == userId).First();
                user = daycare.UserId;
            }
            else if (isUser("Employee"))
            {
                user = db.Employees.Where(e => e.UserId.Id == userId).First().UserId;
                daycare = db.Employees.Where(e => e.UserId.Id == userId).First().DayCareId;
            }
            else if (isUser("Parent"))
            {
                user = db.Parents.Where(p => p.UserId.Id == userId).First().UserId;
                daycare = db.Parents.Where(p => p.UserId.Id == userId).First().DayCare;
            }

            if (daycare != null && user != null)
            {
                var parents = db.DayCares.Where(d => d.Id == daycare.Id).First().Parents.Select(p => p.UserId).ToList();
                var employees = db.DayCares.Where(d => d.Id == daycare.Id).First().Employees.Select(e => e.UserId).ToList();
                var users = parents.Concat(employees).ToList();
                users.Add(daycare.UserId);
                users.Remove(user);

                return View(users);
            }
            TempData["ErrorMessage"] = "**Error: You must be registered with a day care to access a chat.";
            return RedirectToAction("Index", "Users");
        }

        // GET: Messages
        public ActionResult GroupChat()
        {
            TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);

            string channelSid = GetChannelId();

            if (channelSid == null)
            {
                TempData["ErrorMessage"] = "**Error: You must be registered with a day care to access a chat.";
                return RedirectToAction("Index", "Users");
            }

            ViewBag.UserName = User.Identity.Name;
            return View(MessageResource.Read(TwilioApiKeys.serviceSid, channelSid));
        }

        private string GetChannelId()
        {
            string userId = User.Identity.GetUserId();

            // Admin not handled yet
            if (isUser("Admin"))
            {
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
                if(ViewBag.DayCareName == null || ViewBag.DayCareName == "")
                {
                    return null;
                }
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