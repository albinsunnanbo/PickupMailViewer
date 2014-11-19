using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
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
    }
}