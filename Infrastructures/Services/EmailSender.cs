using System.Text.RegularExpressions;
using BDRDExce.Infrastructures.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BDRDExce.Infrastructures.Services
{
    public class EmailSender(IConfiguration configuration, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly string _from = configuration["MailSetting:From"];
        private readonly string _host = configuration["MailSetting:Host"];
        private readonly int _port = int.Parse(configuration["MailSetting:Port"]);
        private readonly string _password = configuration["MailSetting:Password"];
        public void SendEmail(string email, string subject, string body)
        {
            _ = Task.Run(() =>
            {
                var message = CreateMailMessage(email, subject, body);
                try
                {
                    using (var client = new SmtpClient())
                        {
                            client.Connect(_host, _port, SecureSocketOptions.StartTls);
                            client.Authenticate(_from , _password);
                            client.Send(message);
                            client.Disconnect(true);
                        }

                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Send mail fail: {ex.ToString()}");
                }
            });
        }

        private MimeMessage CreateMailMessage(string email, string subject, string content)
        {
            var from = _from;
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(from.Split("@")[0], from));
            mimeMessage.To.Add(new MailboxAddress(email.Split("@")[0], email));
            mimeMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = content;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            return mimeMessage;
        }
    }
}