using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Core.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<LoginCredential> LoginCredention { get; set; }
    }
}
