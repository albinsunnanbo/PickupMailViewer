using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PickupMailViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileList()
        {
            var mailPaths = MailHelper.ListMailFiles(Properties.Settings.Default.MailDir);
            var mails = mailPaths.Select(path => new MailModel(path)).OrderByDescending(m => m.SentOn);
            return View(mails);
        }

        public FileResult DownloadMail(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailid", mailId);
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId + ".eml");
            var result = new FileStreamResult(new FileStream(filePath, FileMode.Open), "message/rfc822");
            result.FileDownloadName = mailId + ".eml";
            return result;
        }
    }
}