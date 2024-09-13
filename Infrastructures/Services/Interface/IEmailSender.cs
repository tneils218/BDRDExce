namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string body);
    }
}