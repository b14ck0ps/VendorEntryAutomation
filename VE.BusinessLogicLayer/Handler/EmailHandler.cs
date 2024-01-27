using System;
using System.Configuration;

namespace VE.BusinessLogicLayer.Handler
{
    public class EmailHandler
    {
        public static bool SendEmail(string toAddress, string title, string code, string status, string link)
        {
            var fromAddress = ConfigurationManager.AppSettings["EmailfromAddress"];
            var subject = ConfigurationManager.AppSettings["Emailsubject"];
            var htmlBody = EmailBody(code, title, status, link);
            return Utilities.Email.Send(fromAddress, toAddress, subject, htmlBody);
        }

        private static string EmailBody(string code, string title, string status, string link)
        {
            return $"Dear Mr/Ms, {title} <br> " +
                   "<p> A new vendor enlistment request has been submitted with the following details:</p>" +
                   $"<p>Vendor Code: {code}</p>" +
                   $"<p>Status: {status}</p>" +
                   $"<p>Click <a href='{link}'>here</a> to view the details</p>" +
                   "<br><i>This is an auto generated email. Please do not reply.</i>";
        }
    }
}
