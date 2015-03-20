using PickupMailViewer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Models
{
    public class MailModel : MessageModel
    {
        private readonly CDO.Message mail;
        private readonly string mailPath;
        public MailModel(string mailPath)
        {
            this.mailPath = mailPath;
            this.mail = MailHelper.ReadMessage(mailPath);
        }

        [Newtonsoft.Json.JsonIgnore]
        override public DateTime SentOn
        {
            get
            {
                return mail.SentOn;
            }
        }

        override public string ToAddress
        {
            get
            {
                return string.Join(", ", mail.To.Split(',').Select(a => new System.Net.Mail.MailAddress(a).Address));
            }
        }

        override public string FromAddress
        {
            get
            {
                return new System.Net.Mail.MailAddress(mail.From).Address;
            }
        }


        override public string Subject
        {
            get
            {
                return mail.Subject;
            }
        }

        override public bool BodyIsHTML { get { return (!string.IsNullOrEmpty(mail.HTMLBody)); } }

        [Newtonsoft.Json.JsonIgnore]
        override public string Body
        {
            get
            {
                if (BodyIsHTML)
                {
                    return mail.HTMLBody;
                }
                return mail.TextBody;
            }
        }

        public string MailId
        {
            get
            {
                return Path.GetFileName(mailPath);
            }
        }
    }
}