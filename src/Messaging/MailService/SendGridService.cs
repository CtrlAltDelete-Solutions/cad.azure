using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using SendGrid;

namespace CAD.Azure
{
    public class SendGridService : IMailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public SendGridService(string smtpServer, string username, string password)
        {
            this._smtpServer = smtpServer;
            this._smtpUser = username;
            this._smtpPassword = password;
        }

        public void SendSMTP(MailMessage message)
        {
            throw new NotImplementedException();
        }


        public void SendSMTP(string from, string to, string subject, string message, string cc = null, string bcc = null, string attachments = null, string alternateText = null, bool trackLinks = false)
        {
            SendGridMessage mailMessage = new SendGridMessage();
            MailAddress fromAddress = new MailAddress(from);


            List<string> recipients = to.Replace(",", ";").Split(';').ToList();
            
            mailMessage.From = fromAddress;
            mailMessage.AddTo(recipients);
            mailMessage.Subject = subject;
            mailMessage.Html = message;
            mailMessage.Text = alternateText;
            
            if(!String.IsNullOrEmpty(attachments))
            {
                List<string> attachmentFiles = attachments.Replace(",", ";").Split(';').ToList();
                foreach(string attachmentFile in attachmentFiles)
                {
                    mailMessage.AddAttachment(attachmentFile);
                }
            }

            mailMessage.EnableClickTracking(trackLinks);

            NetworkCredential credentials = new NetworkCredential(this._smtpUser, this._smtpPassword);
            var webTransport = new Web(credentials);

            webTransport.DeliverAsync(mailMessage).Wait();
        }
    }
}
