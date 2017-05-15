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
using KidCloudProject._APIs;

namespace KidCloudProject.Hubs
{
    public class GroupChatHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser User;

        public void Send(string body, string userName)
        {
            this.User = db.Users.Where(u => u.UserName == userName).First();

            TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);

            string channelSid = GetGroupChannelId();
            
            if (channelSid != null && body != "" && body != null)
            {
                try
                {
                    MessageResource.Create(TwilioApiKeys.serviceSid, channelSid, body, this.User.UserName);
                    Clients.All.addNewMessageToPage(userName, body, DateTime.Now.ToString("h:mm tt"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetGroupChannelId()
        {
            string userId = this.User.Id;

            // Admin not handled yet
            if (isUser("Admin"))
            {
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
                return db.Parents.Where(p => p.UserId.Id == userId).First().DayCare.ChannelId;
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