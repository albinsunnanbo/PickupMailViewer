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
            const int batchSize = 10;
            var rest = HomeController.GetMessageListModel().SkipWhile(m => m.Model.Value.MessageId != lastMessageId);

            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub, ClientInterface>();
            var batch = new List<MessageModel>();
            foreach (var r in rest)
            {
                batch.Add(r.Model.Value);

                if (batch.Count > batchSize)
                {
                    // Send batch
                    context.Clients.All.newMessage(batch, false);
                    batch = new List<MessageModel>();
                    System.Threading.Thread.Sleep(25);
                }
            }
            if (batch.Any())
            {
                // send remaining
                context.Clients.All.newMessage(batch, false);
            }
        }
    }
}