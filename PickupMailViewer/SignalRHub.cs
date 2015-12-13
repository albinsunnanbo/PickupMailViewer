using Microsoft.AspNet.SignalR;
using PickupMailViewer.Controllers;
using PickupMailViewer.Helpers;
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
        void newMessage(MessageModel message, bool onTop, string subPath);

        void newMessage(IEnumerable<MessageModel> messages, bool onTop, string subPath);
    }

    public class SignalRHub : Hub<ClientInterface>
    {
        public void GetRest(string lastMessageId, string path)
        {
            const int batchSize = 10;
            var rest = HomeController.GetMessageListModel(path);
            if (!string.IsNullOrEmpty(lastMessageId))
            {
                rest = rest
                    .SkipWhile(m => m.Model.Value.MessageId != lastMessageId)
                    .Skip(1); // First is the matched message
            }

            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub, ClientInterface>();
            var batch = new List<MessageModel>();
            foreach (var r in rest)
            {
                batch.Add(r.Model.Value);

                if (batch.Count > batchSize)
                {
                    // Send batch
                    context.Clients.All.newMessage(batch, false, path);
                    batch = new List<MessageModel>();
                    System.Threading.Thread.Sleep(25);
                }
            }
            if (batch.Any())
            {
                // send remaining
                context.Clients.All.newMessage(batch, false, path);
            }
        }
    }
}