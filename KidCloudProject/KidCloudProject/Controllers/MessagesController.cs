﻿using System;
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

        public ActionResult CreateDirectMessageChannel(string Username)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser reciver = db.Users.Where(u => u.UserName == Username).First();
            ApplicationUser sender = db.Users.Where(u => u.Id == userId).First();

            ViewBag.UserName = User.Identity.Name;
            DirectMessageChannel dmChannel = null;

            try
            {
                dmChannel = db.DirectMessageChannels.Where(dm => dm.ReciverId.Id == reciver.Id && dm.SenderId.Id == sender.Id || dm.ReciverId.Id == sender.Id && dm.SenderId.Id == reciver.Id).First();
                return View("GroupChat", MessageResource.Read(TwilioApiKeys.serviceSid, dmChannel.ChannelId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            

            if (dmChannel != null)
            {
                // API Create channel
                TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);
                var channel = ChannelResource.Create(TwilioApiKeys.serviceSid, friendlyName: "DM Channel", type: ChannelResource.ChannelTypeEnum.Private);

                // Database DMChannel
                dmChannel = new DirectMessageChannel();
                dmChannel.ChannelId = channel.Sid;
                dmChannel.ReciverId = reciver;
                dmChannel.SenderId = sender;

                // API Add members
                MemberResource.Create(TwilioApiKeys.serviceSid, channel.Sid, dmChannel.ReciverId.UserName);
                MemberResource.Create(TwilioApiKeys.serviceSid, channel.Sid, dmChannel.SenderId.UserName);

                // Save DMChannel to Database
                db.DirectMessageChannels.Add(dmChannel);
                db.SaveChanges();

                return View("GroupChat", MessageResource.Read(TwilioApiKeys.serviceSid, channel.Sid));
            }

            return Content("Something broke");
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


            return Content("We broke something");
        }

        // GET: Messages
        public ActionResult GroupChat()
        {
            TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);

            string channelSid = GetChannelId();

            if (channelSid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = User.Identity.Name;
            return View(MessageResource.Read(TwilioApiKeys.serviceSid, channelSid));
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