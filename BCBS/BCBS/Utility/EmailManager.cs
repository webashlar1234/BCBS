using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BCBS.Utility
{
    public class EmailManager
    {
        public SmtpClient GetSMTP()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("webashlar.developers", "webashlar@123");// Enter seders User name and password
            smtp.EnableSsl = true;
            return smtp;

        }
        public MailMessage SetMailMessage(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            string Body = body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            return mail;
        }
        public bool SendMail(SmtpClient smtp, MailMessage mail)
        {
            bool result = false;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
