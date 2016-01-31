using Microsoft.AspNet.SignalR;
using PickupMailViewer.Controllers;
using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PickupMailViewer
{
    public interface ClientInterface
    {
        void newMessage(MessageModel message, bool onTop, string subPath);

        void newMessage(IEnumerable<MessageModel> messages, bool onTop, string subPath);

        void onServerReconnectEvent();
    }

    public class SignalRHub : Hub<ClientInterface>
    {
        public override Task OnReconnected()
        {
            this.Clients.Caller.onServerReconnectEvent();
            return base.OnReconnected();
        }

        public void GetRest(string lastMessageId, string path)
        {
            var caller = this.Clients.Caller;
            Task.Factory.StartNew(() =>
            {
                // Async to avoid blocking the GetRest call
                const int batchSize = 10;
                var rest = HomeController.GetMessageListModel(path);
                if (!string.IsNullOrEmpty(lastMessageId))
                {
                    rest = rest
                        .SkipWhile(m => m.Model.Value.MessageId != lastMessageId)
                        .Skip(1); // First is the matched message
                }

                var batch = new List<MessageModel>();
                foreach (var r in rest)
                {
                    batch.Add(r.Model.Value);

                    if (batch.Count > batchSize)
                    {
                        // Send batch
                        caller.newMessage(batch, false, path);
                        batch = new List<MessageModel>();
                        System.Threading.Thread.Sleep(25);
                    }
                }
                if (batch.Any())
                {
                    // send remaining
                    caller.newMessage(batch, false, path);
                }
            });
        }
    }
}