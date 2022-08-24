using PasswordManager.Core.Application.QueryParameters;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Core.Application.Interfaces
{
    public interface IPasswordDetailsRepository
    {
        Task AddNewCredentials(LoginCredential loginCredential);
        Task UpdateCredentials(int id, LoginCredential loginCredential);
        Task RemoveCredentials(int credId);
        Task<LoginCredential> GetCredential(string id, int credId);
        Task<PagedList<LoginCredential>> GetCredentials(string id, QueryStringParameters queryStringParameters);
    }
}
