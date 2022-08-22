namespace PasswordManager.Api.Dtos
{
    public class LoginCredentialDto
    {
        public int Id { get; set; }
        public string WebsiteName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string WebAddress { get; set; }
        public string Note { get; set; }
        public string ImageUrl { get; set; }
        //public int? UserId { get; set; }
       // public AppUser AppUser { get; set; }
    }
}
