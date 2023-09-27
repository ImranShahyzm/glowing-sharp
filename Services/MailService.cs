using SagErpBlazor.ExtendedClasses;
using System.Net.Mail;
using System.Net;

namespace SagErpBlazor.Services
{
    public class MailService: IMailInterface
    {
        private readonly MailSettings _mailConfig;
        public MailService(MailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public async Task<string> SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
        {
            string Responce = "";
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailConfig.FromEmail);
                message.To.Add(new MailAddress(ToEmail));
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = HTMLBody;
                smtp.Port = _mailConfig.Port;
                smtp.Host = _mailConfig.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
                Responce = "";
            }
            catch (Exception e)
            {
                Responce = e.Message;
            }

            return Responce;
        }
    }
}
