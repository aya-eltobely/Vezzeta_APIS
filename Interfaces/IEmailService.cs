namespace VezetaApi.Interfaces
{
    public interface IEmailService
    {
        void SendWelcomeEmail(string toEmail, string userName, string password);
    }
}
