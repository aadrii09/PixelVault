namespace ApiVault.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendWelcomeEmailAsync(string toEmail);
    }
}
