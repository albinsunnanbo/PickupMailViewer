﻿using PickupMailViewer.Helpers;
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
        public MailModel(string mailPath) : base(mailPath)
        {
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
                return string.Join(", ", mail.To.Split(',').Where(a => a.Length > 0).Select(a => new System.Net.Mail.MailAddress(a).Address));
            }
        }

        override public string FromAddress
        {
            get
            {
                if (string.IsNullOrEmpty(mail.From))
                {
                    return string.Empty;
                }
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

        public override string MessageType
        {
            get
            {
                return "Mail";
            }
        }

        public override string[] AttachmentNames
        {
            get
            {
                var attachments = mail.Attachments.OfType<CDO.IBodyPart>();
                return attachments.Select(a => a.FileName).ToArray();
            }
        }

        public override string[] AttachmentSizes
        {
            get
            {
                var attachments = mail.Attachments.OfType<CDO.IBodyPart>();
                return attachments.Select(a =>
                {
                    ADODB.Stream stm = a.GetDecodedContentStream();
                    var len = stm.Size;
                    stm.Close();
                    return FormatLength(len);
                }).ToArray();
            }
        }

        private string FormatLength(int length)
        {
            if(length >= 1024*1024)
            {
                return Math.Round(length * 1.0 / (1024 * 1024), 1) + "MB";
            }
            else if (length >= 1024 )
            {
                return Math.Round(length * 1.0 / (1024 ), 1) + "kB";
            }
            return length + "B";
        }

        public string GetAttachmentMediaTypeFromIdx(int idx)
        {
            var attachments = mail.Attachments.OfType<CDO.IBodyPart>();
            var attachment = attachments.Skip(idx).FirstOrDefault();
            return attachment.ContentMediaType;
        }

        public byte[] GetAttachmentContentFromIdx(int idx)
        {
            var attachments = mail.Attachments.OfType<CDO.IBodyPart>();
            var attachment = attachments.Skip(idx).FirstOrDefault();

            ADODB.Stream stm = attachment.GetDecodedContentStream();

            // cast to COM IStream and load into byte array
            var comStream = (System.Runtime.InteropServices.ComTypes.IStream)stm;
            byte[] attachmentData = new byte[stm.Size];
            comStream.Read(attachmentData, stm.Size, IntPtr.Zero);

            stm.Close();
            return attachmentData;
        }
    }
}