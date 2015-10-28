using Microsoft.AspNet.SignalR;
using PickupMailViewer.Controllers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer
{
    public class SignalRHub : Hub
    {
        public void GetRest(string lastMessageId)
        {
            var rest = HomeController.GetMessageListModel().SkipWhile(m => m.Model.Value.MessageId != lastMessageId);

            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
            foreach (var r in rest)
            {
                System.Threading.Thread.Sleep(5);
                context.Clients.All.oldMessage(r.Model.Value);
            }
        }
    }
}