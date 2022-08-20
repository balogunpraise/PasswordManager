using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Core.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<LoginCredential> LoginCredention { get; set; } = new List<LoginCredential>();
    }
}
