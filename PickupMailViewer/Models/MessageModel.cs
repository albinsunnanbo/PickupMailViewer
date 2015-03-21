using System;

namespace PickupMailViewer.Models
{
    public abstract class MessageModel
    {
        public virtual string Body { get { return ""; } }

        public virtual bool BodyIsHTML { get { return false; } }

        public abstract string FromAddress { get; }

        public abstract DateTime SentOn { get; }

        public string SentOnFormatted
        {
            get
            {
                return SentOn.ToString();
            }
        }

        public abstract string Subject { get; }

        public abstract string ToAddress { get; }

        public abstract string MessageType { get; }
    }
}
