using AutoMapper;
using PasswordManager.Api.Dtos;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LoginCredentialDto, LoginCredential>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, UserDto>();
        }
    }
}
