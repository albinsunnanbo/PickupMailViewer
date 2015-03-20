using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Helpers
{
    public static class SmsHelper
    {
        public static IEnumerable<string> ListSmsFiles(string path)
        {
            return Directory.EnumerateFiles(path, "*.sms");
        }
    }
}