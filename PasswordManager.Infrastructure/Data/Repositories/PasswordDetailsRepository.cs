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

        public async Task AddNewCredentials(LoginCredential loginCredential)
        {
            await _context.LoginCredentials.AddAsync(loginCredential);
            await _context.SaveChangesAsync();
        }

        public async Task<LoginCredential> GetCredential(int credId)
        {
            LoginCredential? loginCredential = await _context.LoginCredentials.FindAsync(credId);
            return loginCredential;
        }

        public async Task RemoveCredentials(int credId)
        {
            var credential = await _context.LoginCredentials.FindAsync(credId);
            if (credential != null)
                _context.Remove(credential);
            await _context.SaveChangesAsync();
        }

        public Task UpdateCredentials(string credId, LoginCredential loginCredential)
        {
            throw new NotImplementedException();
        }
    }
}
