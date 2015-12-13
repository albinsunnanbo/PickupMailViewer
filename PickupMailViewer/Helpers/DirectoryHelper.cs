using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Helpers
{
    public static class DirectoryHelper
    {
        public static IEnumerable<string> ListDirectories()
        {
            var path = Properties.Settings.Default.MailDir;
            return new DirectoryInfo(path).EnumerateDirectories()
                .Where(d => d.EnumerateFiles(Properties.Settings.Default.FilePattern).Any() || d.EnumerateFiles("*.sms").Any())
                .Select(d => d.Name);
        }
    }
}