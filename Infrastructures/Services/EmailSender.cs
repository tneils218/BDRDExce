using System.Text.RegularExpressions;
using BDRDExce.Infrastructures.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BDRDExce.Infrastructures.Services
{
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly string _from = configuration["MailSetting:From"];
        private readonly string _host = configuration["MailSetting:Host"];
        private readonly int _port = int.Parse(configuration["MailSetting:Port"]);
        private readonly string _password = configuration["MailSetting:Password"];
        public void SendEmailAsync(string email, string subject, string body)
        {
            _ = Task.Run(() =>
            {
                var message = CreateMailMessage(email, subject, body);
                try
                {
                    using (var client = new SmtpClient())
                        {
                            client.CheckCertificateRevocation = false;
                            client.Connect(_host, _port, SecureSocketOptions.StartTls);
                            client.Authenticate(_from , _password);
                            client.Send(message);
                            client.Disconnect(true);
                        }

                }
                catch (Exception ex)
                {
                    throw new Exception($"Send mail fail: {ex.ToString()}");
                }
            });
        }

        private MimeMessage CreateMailMessage(string email, string subject, string content)
        {
            var to = Regex.Replace(email, "[\\s]+", "").Split(",").Select(x => x).ToArray();
            var from = _from;
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(from.Split("@")[0], from));
            foreach (var t in to)
                mimeMessage.To.Add(new MailboxAddress(t.Split("@")[0], t));
            mimeMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = content;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            return mimeMessage;
        }
    }
}