using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Twilio;
using Twilio.Rest.Chat.V2.Service.Channel;
using Microsoft.AspNet.Identity;
using KidCloudProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KidCloudProject.Hubs
{
    public class ChatHub : Hub
    {
        const string accountSid = "AC383fc67c15e32582075da33d541fd388";
        const string authToken = "7915107c0ef796ad3d5a51fe2cdd0552";
        const string serviceSid = "IS9acc1cbb2d874347a95cacebfc5cd5aa";
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser User;

        public void Send(string body, string userName)
        {
            this.User = db.Users.Where(u => u.UserName == userName).First();

            TwilioClient.Init(accountSid, authToken);

            string channelSid = GetChannelId();
            
            if (channelSid != null && body != "" && body != null)
            {
                try
                {
                    MessageResource.Create(serviceSid, channelSid, body, this.User.UserName);
                    Clients.All.addNewMessageToPage(userName, body, DateTime.Now.ToString("h:mm tt"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetChannelId()
        {
            string userId = this.User.Id;

            if (isUser("Admin"))
            {
                //channelSid = db.Parents.Where(p => p.UserId.Id == userId).First().DayCareId.ChannelId;
                return null;
            }
            else if (isUser("DayCare"))
            {
                return db.DayCares.Where(e => e.UserId.Id == userId).First().ChannelId;
            }
            else if (isUser("Employee"))
            {
                return db.Employees.Where(e => e.UserId.Id == userId).First().DayCareId.ChannelId;
            }
            else if (isUser("Parent"))
            {
                return db.Parents.Where(p => p.UserId.Id == userId).First().DayCareId.ChannelId;
            }

            return null;
        }

        private bool isUser(string role)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var s = UserManager.GetRoles(this.User.Id);
            if (s[0].ToString() == role)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}