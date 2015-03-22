using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickupMailViewer.Helpers
{
    public class SmsData
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime SentOn { get; set; }
    }
}