using PickupMailViewer.Helpers;
using PickupMailViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PickupMailViewer.Controllers
{
    public class HomeController : Controller
    {
        const int initialPageSize = 20;
        public ActionResult Index(string id)
        {
            ViewData["subPath"] = id;
            return View(GetMessageListModel(id).Take(20));
        }

        public static IEnumerable<MessageTimeModel> GetMessageListModel(string subPath)
        {
            if (!(string.IsNullOrEmpty(subPath) || DirectoryHelper.ListDirectories().Contains(subPath)))
            {
                throw new ArgumentException("Invalid path", nameof(subPath));
            }
            var mailDir = Path.Combine(Properties.Settings.Default.MailDir, subPath ?? "");
            var mailPaths = MailHelper.ListMailFiles(mailDir);
            var mails = mailPaths.Select(path => new MessageTimeModel { Model = new Lazy<MessageModel>(() => new MailModel(path.FullName)), MessageTime = path.CreationTimeUtc });

            var smsPaths = SmsHelper.ListSmsFiles(mailDir);
            var sms = smsPaths.Select(path => new MessageTimeModel { Model = new Lazy<MessageModel>(() => new SmsModel(path.FullName)), MessageTime = path.CreationTimeUtc });

            var messages = mails.Concat(sms)
                .OrderByDescending(m => m.MessageTime);

            return messages;
        }

        public ActionResult Search(string searchText, string subPath)
        {
            searchText = searchText.ToLowerInvariant();
            var messageModels = MailHelper.SearchCache(subPath).Concat(SmsHelper.SearchCache(subPath));
            var result = messageModels
                .Where(smsModel =>
                smsModel.ToAddress.ToLowerInvariant().Contains(searchText) ||
                (smsModel.FromAddress != null && smsModel.FromAddress.ToLowerInvariant().Contains(searchText)) ||
                (smsModel.Subject != null && smsModel.Subject.ToLowerInvariant().Contains(searchText)) ||
                smsModel.Body.ToLowerInvariant().Contains(searchText)
                )
                .Select(m => m.MessageId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadMail(string mailId, string subPath)
        {
            ValidateMailId(mailId);
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, mailId);
            if (!MailHelper.ListMailFiles(Properties.Settings.Default.MailDir).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", nameof(mailId));
            }
            var result = new FileStreamResult(new FileStream(filePath, FileMode.Open), "message/rfc822");
            result.FileDownloadName = mailId;
            return result;
        }

        public FileResult DownloadMailAttachment(string mailId, string subPath, int idx)
        {
            ValidateMailId(mailId);
            subPath = subPath ?? "";
            ValidateSubPath(subPath);
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, subPath, mailId);
            if (!MailHelper.ListMailFiles(Path.Combine(Properties.Settings.Default.MailDir, subPath)).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", nameof(mailId));
            }

            var mail = new MailModel(filePath);

            var result = File(mail.GetAttachmentContentFromIdx(idx), mail.GetAttachmentMediaTypeFromIdx(idx));
            result.FileDownloadName = mail.AttachmentNames[idx];
            return result;
        }


        public ActionResult PreviewMailAttachment(string mailId, string subPath, int idx)
        {
            ValidateMailId(mailId);
            subPath = subPath ?? "";
            ValidateSubPath(subPath);
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, subPath, mailId);
            if (!MailHelper.ListMailFiles(Path.Combine(Properties.Settings.Default.MailDir, subPath)).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", nameof(mailId));
            }

            var mail = new MailModel(filePath);

            return Content(Encoding.UTF8.GetString(mail.GetAttachmentContentFromIdx(idx)), mail.GetAttachmentMediaTypeFromIdx(idx));
        }

        [OutputCache(Location = OutputCacheLocation.Downstream, VaryByParam = "*", Duration = 3600 * 24 * 7)] // No need to output cache on the server since the mail content is cached internally anyway
        public ActionResult GetMailDetails(string mailId, string subPath)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", "mailId");
            }
            ValidateSubPath(subPath);
            ViewData["subPath"] = subPath;
            var filePath = Path.Combine(Properties.Settings.Default.MailDir, subPath, mailId);
            if (!MailHelper.ListMailFiles(Path.Combine(Properties.Settings.Default.MailDir, subPath)).Select(f => f.FullName).Contains(filePath))
            {
                throw new ArgumentException("mailId is not in white list", "mailId");
            }
            return PartialView(new MailModel(filePath));
        }

        private static void ValidateMailId(string mailId)
        {
            if (mailId.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in mailId", nameof(mailId));
            }
        }

        private static void ValidateSubPath(string subPath)
        {
            if (subPath.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new ArgumentException("Invalid characters in subPath", nameof(subPath));
            }
            if (!(string.IsNullOrEmpty(subPath) || DirectoryHelper.ListDirectories().Contains(subPath)))
            {
                throw new ArgumentException("Invalid path", nameof(subPath));
            }
        }
    }
}