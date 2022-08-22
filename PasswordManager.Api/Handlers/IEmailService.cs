namespace PasswordManager.Api.Handlers
{
    public interface IEmailService
    {
        Task SendEmail(EmailModel emailModel);
    }
}
