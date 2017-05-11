using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace KidCloudProject.Hubs
{
    public class TestHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
            Clients.All.addNewMessageTest(name, message);
        }
    }
}