using BCBS.Models;
using Mvc.Mailer;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace BCBS.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome()
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                x.To.Add("some-email@example.com");
            });
        }

        public virtual MvcMailMessage GoodBye()
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "GoodBye";
                x.ViewName = "GoodBye";
                x.To.Add("some-email@example.com");
            });
        }
        public virtual MvcMailMessage SBF(SBFEmailViewModel sbf,HttpPostedFileBase file,string subject)
        {
            string receviermail = WebConfigurationManager.AppSettings["ToEmail"];
            ViewData.Model = sbf;
            if (file != null && file.ContentLength > 0)
            {
                var attachment = new Attachment(file.InputStream,sbf.Invoice.SupportingDocuments);
                return Populate(x =>
                {
                    x.ViewName = "sbf";
                    x.To.Add("dharmin.naik308@gmail.com");
                    x.CC.Add(receviermail);
                    x.Subject = subject;
                    x.Attachments.Add(attachment);
                });
            }
            else
            {
                return Populate(x =>
                {
                    x.ViewName = "sbf";
                    x.To.Add("dharmin.naik308@gmail.com");
                    x.CC.Add(receviermail);
                    x.Subject = subject;

                });
            }
            //return PopulateBody(mailMessage, "SBF");
        }
        //public virtual MailMessage SBF()
        //{
        //    MvcMailMessage mailMessage = new MvcMailMessage();
        //    mailMessage.Subject = "BCBS SBF";
        //    PopulateBody(mailMessage, viewName: "Welcome");
        //    return mailMessage;
        //}
    }
}