using AutoMapper;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Profiles;

namespace Najam.TaskBook.WebApi.Mappings
{
    public class TypeMappingProfile : Profile
    {
        public TypeMappingProfile()
        {
            CreateMap<User, ProfileViewModel>();
        }
    }
}
