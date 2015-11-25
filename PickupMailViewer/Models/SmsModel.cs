using Newtonsoft.Json;
using PickupMailViewer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Models
{
    public class SmsModel : MessageModel
    {
        readonly SmsData loadedData;

        public SmsModel(string path) : base(path)
        {
            loadedData = SmsHelper.ReadMessage(path);
        }

        override public string FromAddress
        {
            get
            {
                return loadedData.From;
            }
        }

        override public DateTime SentOn
        {
            get
            {
                return loadedData.SentOn;
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

        public override string MessageType
        {
            get
            {
                return "Sms";
            }
        }

        public override string[] AttachmentNames
        {
            get
            {
                return new string[0];
            }
        }
        public override string[] AttachmentSizes
        {
            get
            {
                return new string[0];
            }
        }
    }
}