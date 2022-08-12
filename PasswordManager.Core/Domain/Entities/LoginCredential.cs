using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Domain.Entities
{
    public class LoginCredential : BaseEntity
    {
        public string WebsiteName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string WebAddress { get; set; }
        public string Note { get; set; }
        public string ImageUrl { get; set; }
        public AppUser AppUser { get; set; }
    }
}
