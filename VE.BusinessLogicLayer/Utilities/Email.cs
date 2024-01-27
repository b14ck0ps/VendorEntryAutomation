using System.Net.Mail;
using System.Net;
using System;
using System.Configuration;

namespace VE.BusinessLogicLayer.Utilities
{
    public class Email
    {
        public static bool Send(string fromAddress, string toAddress, string subject, string htmlBody)
        {
            try
            {
                var host = ConfigurationManager.AppSettings["SMTP_Host"];
                var port = ConfigurationManager.AppSettings["SMTP_Port"];

                var username = ConfigurationManager.AppSettings["SMTP_Username"];
                var password = ConfigurationManager.AppSettings["SMTP_Password"];

                var client = new SmtpClient(host, Convert.ToInt32(port))
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                var mailMessage = new MailMessage();
                mailMessage.AlternateViews.Add(htmlView);
                mailMessage.From = new MailAddress(fromAddress);
                mailMessage.To.Add(new MailAddress(toAddress));
                mailMessage.Subject = subject;

                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}