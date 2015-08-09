using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace CAD.Azure
{
    public interface IMailService
    {
        void SendSMTP(MailMessage message);
        void SendSMTP(string from, string to, string subject, string message, string cc=null,string bcc=null, string attachments=null, string alternateText = null, bool trackLinks = false);
    }
}
