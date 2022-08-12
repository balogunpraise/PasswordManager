using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;
using PasswordManager.Infrastructure.Data.Context;

namespace PasswordManager.Infrastructure.Data.Repositories
{
    public class PasswordDetailsRepository : IPasswordDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddNewCredentials(LoginCredential loginCredential)
        {
            throw new NotImplementedException();
        }

        public Task<LoginCredential> GetCredential(string credId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCredentials(string credId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCredentials(string credId, LoginCredential loginCredential)
        {
            throw new NotImplementedException();
        }
    }
}
