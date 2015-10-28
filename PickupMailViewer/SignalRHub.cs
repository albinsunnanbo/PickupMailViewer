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
    public interface ClientInterface
    {
        void newMessage(MessageModel message, bool onTop);

        void newMessage(IEnumerable<MessageModel> messages, bool onTop);
    }

    public class SignalRHub : Hub<ClientInterface>
    {
        public void GetRest(string lastMessageId)
        {
            var rest = HomeController.GetMessageListModel().SkipWhile(m => m.Model.Value.MessageId != lastMessageId);

            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub, ClientInterface>();
            foreach (var r in rest)
            {
                System.Threading.Thread.Sleep(5);
                context.Clients.All.newMessage(r.Model.Value, false);
            }
        }
    }
}