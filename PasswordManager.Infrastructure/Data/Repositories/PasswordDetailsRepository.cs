using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Application;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Application.QueryParameters;
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

        public async Task<LoginCredential> GetCredential(string id, int credId)
        {
            LoginCredential? loginCredential = await _context.LoginCredentials
                .Where(x => x.UserId == id && x.Id == credId).SingleOrDefaultAsync();
            return loginCredential;
        }

        public PagedList<LoginCredential>GetCredentials(string id, QueryStringParameters queryStringParameters)
        {
            return PagedList<LoginCredential>.ToPagedList(_context.LoginCredentials.Where(x => x.UserId == id)
                .OrderBy(x => x.WebsiteName)
                .Skip((queryStringParameters.PageNumber - 1) * queryStringParameters.PageSize)
                .Take(queryStringParameters.PageSize),queryStringParameters.PageNumber, queryStringParameters.PageSize);
        }
        public async Task RemoveCredentials(int credId)
        {
            var credential = await _context.LoginCredentials.FindAsync(credId);
            if (credential != null)
                _context.Remove(credential);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCredentials(int id, LoginCredential loginCredential)
        {
            var res = await _context.LoginCredentials.FindAsync(id);
            if (res != null)
            {
                res.WebsiteName = loginCredential.WebsiteName;
                res.Password =  loginCredential.Password;
                res.Email = loginCredential.Email;
                res.Note = loginCredential.Note;
                res.WebAddress = loginCredential.WebAddress;
            }
            await _context.SaveChangesAsync();
        }
    }
}
