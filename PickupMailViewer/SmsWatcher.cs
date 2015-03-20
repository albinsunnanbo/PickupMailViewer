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
            fsw = new FileSystemWatcher(Properties.Settings.Default.MailDir, "*.json");
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
                int failCount = 0;
                SmsModel sms = null;

                while (sms == null)
                {
                    try
                    {
                        sms = new SmsModel(e.FullPath);
                    }
                    catch (IOException ex)
                    {
                        // There's a race condition when the file system watcher
                        // is so fast that it tries to read the file before it
                        // has been completely weritten. Retry for a few times
                        // in that case. HResult 0x80070020 = -2147024864 is
                        // file is locked.
                        if (ex.HResult == -2147024864 && failCount++ <= 100)
                        {
                            Thread.Sleep(10);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                context.Clients.All.newMessage(sms);
            }
        }
    }
}