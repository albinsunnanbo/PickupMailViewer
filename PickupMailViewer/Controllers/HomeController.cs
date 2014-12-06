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
            return View(GetMailListModel());
        }

        public ActionResult FileList()
        {
            return View(GetMailListModel());
        }

        private static IOrderedEnumerable<MailModel> GetMailListModel()
        {
            var mailPaths = MailHelper.ListMailFiles(Properties.Settings.Default.MailDir);
            var mails = mailPaths.Select(path => new MailModel(path)).OrderByDescending(m => m.SentOn);
            return mails;
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

        [OutputCache(Location = OutputCacheLocation.Downstream, VaryByParam = "mailId", Duration = 3600 * 24 * 7)] // No need to output cache on the server since the mailcontent is cached internally anyway
        public ActionResult GetMailDetails(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in while list", "mailId");
            }
            return PartialView(new MailModel(filePath));
        }
    }
}