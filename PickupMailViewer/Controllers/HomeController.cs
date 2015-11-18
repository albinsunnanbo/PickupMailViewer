using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PickupMailViewer.Controllers
{
    public class HomeController : Controller
    {
        const int initialPageSize = 20;
        public ActionResult Index()
        {
            return View(GetMessageListModel().Take(20));
        }

        public static IEnumerable<MessageTimeModel> GetMessageListModel()
        {
            var mailPaths = MailHelper.ListMailFiles(Properties.Settings.Default.MailDir);
            var mails = mailPaths.Select(path => new MessageTimeModel { Model = new Lazy<MessageModel>(() => new MailModel(path.FullName)), MessageTime = path.CreationTimeUtc });

            var smsPaths = SmsHelper.ListSmsFiles(Properties.Settings.Default.MailDir);
            var sms = smsPaths.Select(path => new MessageTimeModel { Model = new Lazy<MessageModel>(() => new SmsModel(path.FullName)), MessageTime = path.CreationTimeUtc });

            var messages = mails.Concat(sms)
                .OrderByDescending(m => m.MessageTime);

            return messages;
        }

        public FileResult DownloadMail(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }
            var result = new FileStreamResult(new FileStream(filePath, FileMode.Open), "message/rfc822");
            result.FileDownloadName = mailId;
            return result;
        }

        public FileResult DownloadMailAttachment(string mailId, int idx)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }

            var mail = new MailModel(filePath);

            var result = File(mail.GetAttachmentContentFromIdx(idx), mail.GetAttachmentMediaTypeFromIdx(idx));
            result.FileDownloadName = mail.AttachmentNames[idx];
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
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }
            return PartialView(new MailModel(filePath));
        }
    }
}