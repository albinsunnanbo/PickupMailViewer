using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace PickupMailViewer.Helpers
{
    public static class SmsHelper
    {
        const int maxRetries = 10;

        public static IEnumerable<string> ListSmsFiles(string path)
        {
            return Directory.EnumerateFiles(path, "*.sms");
        }

        private static readonly ConcurrentDictionary<string, SmsData> messageCache
            = new ConcurrentDictionary<string, SmsData>();

        public static SmsData ReadMessage(string path)
        {
            return messageCache.GetOrAdd(path, p => ReadMessageUnCached(p));
        }


        private static SmsData ReadMessageUnCached(string path)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<SmsData>(File.ReadAllText(path));
                    data.SentOn = File.GetCreationTime(path);
                    return data;
                }
                catch (IOException ex)
                {
                    if (i + 1 >= maxRetries || ex.HResult != -2147024864)
                    {
                        // Rethrow last time or if it isn't a file lock problem.
                        throw;
                    }
                }
                Thread.Sleep(100);
            }
            throw new InvalidOperationException("Should not arrive here.");
        }
    }
}