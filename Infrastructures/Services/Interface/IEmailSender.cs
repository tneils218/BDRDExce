namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string body);
    }
}