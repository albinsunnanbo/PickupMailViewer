using Microsoft.AspNet.SignalR;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer
{
    public static class MailWatcher
    {
        private static FileSystemWatcher fsw;
        public static void Init()
        {
            fsw = new FileSystemWatcher(Properties.Settings.Default.MailDir, "*.eml");
            fsw.Created += OnNewMailFileCreated;
            fsw.EnableRaisingEvents = true;
            fsw.Error += OnError;
            fsw.EnableRaisingEvents = true;
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            fsw.Dispose();
            Init();
        }

        private static void OnNewMailFileCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                var mail = new MailModel(e.FullPath);
                var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                context.Clients.All.newMail(mail);
            }
        }
    }
}