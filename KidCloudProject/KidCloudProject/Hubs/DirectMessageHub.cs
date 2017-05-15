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
    public class DirectMessageHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser User;
        private ApplicationUser Receiver;

        public void Send(string body, string sender, string receiver)
        {
            this.User = db.Users.Where(u => u.UserName == sender).First();
            this.Receiver = db.Users.Where(u => u.UserName == receiver).First();

            TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);

            string channelSid = GetDirectMessageChannelId();

            if (channelSid != null && body != "" && body != null)
            {
                try
                {
                    MessageResource.Create(TwilioApiKeys.serviceSid, channelSid, body, this.User.UserName);
                    Clients.All.addNewMessageToPage(sender, body, DateTime.Now.ToString("h:mm tt"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetDirectMessageChannelId()
        {
            try
            {
                return db.DirectMessageChannels.Where(dm => dm.ReciverId.Id == this.Receiver.Id && dm.SenderId.Id == this.User.Id || dm.ReciverId.Id == this.User.Id && dm.SenderId.Id == this.Receiver.Id).First().ChannelId;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}