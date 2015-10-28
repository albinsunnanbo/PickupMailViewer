using Microsoft.AspNet.SignalR;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace PickupMailViewer
{
    public static class SmsWatcher
    {
        private static FileSystemWatcher fsw;
        public static void Init()
        {
            fsw = new FileSystemWatcher(Properties.Settings.Default.MailDir, "*.sms");
            fsw.Created += OnNewSmsFileCreated;
            fsw.Error += OnError;
            fsw.EnableRaisingEvents = true;
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            fsw.Dispose();
            Init();
        }

        private static void OnNewSmsFileCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                var sms = new SmsModel(e.FullPath);

                var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub, ClientInterface>();
                context.Clients.All.newMessage(sms, true);
            }
        }
    }
}