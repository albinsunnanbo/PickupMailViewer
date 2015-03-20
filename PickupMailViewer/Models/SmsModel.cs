using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Models
{
    public class SmsModel : MessageModel
    {
        private class SmsData
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Text { get; set; }
        }

        readonly SmsData loadedData;
        readonly string messageId;
        readonly DateTime sentOn;

        public SmsModel(string path)
        {
            int failCount = 0;

            loadedData = JsonConvert.DeserializeObject<SmsData>(File.ReadAllText(path));
            messageId = Path.GetFileName(path);
            sentOn = File.GetCreationTime(path);
        }

        override public string FromAddress
        {
            get
            {
                return loadedData.From;
            }
        }

        override public string MessageId
        {
            get
            {
                return messageId;
            }
        }

        override public DateTime SentOn
        {
            get
            {
                return sentOn;
            }
        }

        override public string Subject
        {
            get 
            {
                return loadedData.Text;
            }
        }

        override public string ToAddress
        {
            get
            {
                return loadedData.To;
            }
        }
    }
}