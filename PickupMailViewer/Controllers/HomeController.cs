using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PickupMailViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetMessageListModel());
        }

        private static IOrderedEnumerable<MessageModel> GetMessageListModel()
        {
            var mailPaths = MailHelper.ListMailFiles(Properties.Settings.Default.MailDir);
            var mails = mailPaths.Select(path => new MailModel(path));

            var smsPaths = SmsHelper.ListSmsFiles(Properties.Settings.Default.MailDir);
            var sms = smsPaths.Select(path => new SmsModel(path));

            var messages = mails.Cast<MessageModel>().Concat(sms)
                .OrderByDescending(m => m.SentOn);

            return messages;
        }

        public FileResult DownloadMail(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }
            var result = new FileStreamResult(new FileStream(filePath, FileMode.Open), "message/rfc822");
            result.FileDownloadName = mailId;
            return result;
        }

        [OutputCache(Location = OutputCacheLocation.Downstream, VaryByParam = "mailId", Duration = 3600 * 24 * 7)] // No need to output cache on the server since the mail content is cached internally anyway
        public ActionResult GetMailDetails(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }
            return PartialView(new MailModel(filePath));
        }
    }
}