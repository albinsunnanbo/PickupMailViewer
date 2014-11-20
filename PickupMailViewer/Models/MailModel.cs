using PickupMailViewer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Models
{
    public class MailModel
    {
        private readonly CDO.Message mail;
        private readonly string mailPath;
        public MailModel(string mailPath)
        {
            this.mailPath = mailPath;
            this.mail = MailHelper.ReadMessage(mailPath);
        }

        public DateTime SentOn
        {
            get
            {
                return mail.SentOn;
            }
        }

        public string ToAddress
        {
            get
            {
                return new System.Net.Mail.MailAddress(mail.To).Address;
            }
        }

        public string FromAddress
        {
            get
            {
                return new System.Net.Mail.MailAddress(mail.From).Address;
            }
        }


        public string Subject
        {
            get
            {
                return mail.Subject;
            }
        }

        public string Body
        {
            get { return mail.TextBody; }
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