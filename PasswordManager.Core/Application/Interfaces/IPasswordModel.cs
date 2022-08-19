using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Application.Interfaces
{
    internal interface IPasswordModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int? UserId { get; set; }
    }
}
