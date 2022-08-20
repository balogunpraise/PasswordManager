using AutoMapper;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Services;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        private readonly EncryptionService _encyptionService;
        public MappingProfiles()
        {
            _encyptionService = new EncryptionService();
            CreateMap<LoginCredential, LoginCredentialsDto>();
        }
    }
}
