using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace PickupMailViewer.Helpers
{
    // Mail parsing from http://stackoverflow.com/a/19034818/401728
    public static class MailHelper
    {
        const int maxRetries = 10;

        public static IEnumerable<FileInfo> ListMailFiles(string path)
        {
            return new DirectoryInfo(path).EnumerateFiles(Properties.Settings.Default.FilePattern);
        }

        private static readonly ConcurrentDictionary<string, CDO.Message> messageCache =
            new ConcurrentDictionary<string, CDO.Message>();

        public static CDO.Message ReadMessage(String emlFileName)
        {
            return messageCache.GetOrAdd(emlFileName, f => ReadMessageUnCached(f));
        }

        private static CDO.Message ReadMessageUnCached(String emlFileName)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    CDO.Message msg = new CDO.MessageClass();
                    ADODB.Stream stream = new ADODB.StreamClass();
                    stream.Open(Type.Missing,
                                   ADODB.ConnectModeEnum.adModeUnknown,
                                   ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified,
                                   String.Empty,
                                   String.Empty);
                    stream.LoadFromFile(emlFileName);
                    stream.Flush();
                    msg.DataSource.OpenObject(stream, "_Stream");
                    msg.DataSource.Save();
                    return msg;
                }
                catch (COMException ex)
                {
                    if (i + 1 >= maxRetries || ex.HResult != -2146825286)
                    {
                        // Rethrow last time or if it isn't a file lock problem.
                        throw;
                    }
                }
                catch (IOException ex)
                {
                    if (i + 1 >= maxRetries || ex.HResult != -2147024864)
                    {
                        // Rethrow last time or if it isn't a file lock problem.
                        throw;
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            throw new InvalidOperationException("Should not arrive here.");
        }

        public static IEnumerable<string> SearchCache(string searchString, string subPath)
        {
            var basePath = Path.Combine(Properties.Settings.Default.MailDir, subPath);
            foreach (var messagePath in messageCache.Keys)
            {
                var mailModel = new Models.MailModel(messagePath);
                if (messagePath == Path.Combine(basePath, mailModel.MessageId))
                {
                    if (
                        mailModel.ToAddress.Contains(searchString) ||
                        mailModel.FromAddress.Contains(searchString) ||
                        mailModel.Subject.Contains(searchString) ||
                        mailModel.Body.Contains(searchString)
                        )
                    {
                        yield return mailModel.MessageId;
                        continue;
                    }
                }
            }
        }
    }
}