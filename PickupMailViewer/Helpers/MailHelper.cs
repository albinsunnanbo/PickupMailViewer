using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Helpers
{
    // Mail parsing from http://stackoverflow.com/a/19034818/401728
    public static class MailHelper
    {
        public static IEnumerable<string> ListMailFiles(string path)
        {
            return Directory.EnumerateFiles(path, "*.eml");
        }

        public static CDO.Message ReadMessage(String emlFileName)
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
    }
}