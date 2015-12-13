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
            fsw = new FileSystemWatcher(Properties.Settings.Default.MailDir, Properties.Settings.Default.FilePattern);
            fsw.IncludeSubdirectories = true;
            fsw.Created += OnNewMailFileCreated;
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
                var subPath = Path.GetDirectoryName(e.FullPath).Substring(Properties.Settings.Default.MailDir.Length).TrimStart('\\');

                var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub, ClientInterface>();
                context.Clients.All.newMessage(mail, true, subPath);
            }
        }
    }
}