namespace PasswordManager.Api.Dtos
{
    public class LoginCredentialsDto
    {
        public string WebsiteName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string WebAddress { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
