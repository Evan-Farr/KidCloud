using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Chat.V2.Service.Channel;

namespace KidCloudProject.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages
        public ActionResult Index()
        {
            const string accountSid = "AC383fc67c15e32582075da33d541fd388";
            const string authToken = "7915107c0ef796ad3d5a51fe2cdd0552";
            const string serviceSid = "IS9acc1cbb2d874347a95cacebfc5cd5aa";
            const string channelSid = "CH8b3817488d694f2f993e0c381d5346c7";

            TwilioClient.Init(accountSid, authToken);

            var messages = MessageResource.Read(serviceSid, channelSid);

            return View(messages);
        }
    }
}