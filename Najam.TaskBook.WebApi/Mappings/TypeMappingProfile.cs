using AutoMapper;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Profiles;
using Najam.TaskBook.WebApi.Models.UserGroups;
using Najam.TaskBook.WebApi.Parameters.Profiles;

namespace Najam.TaskBook.WebApi.Mappings
{
    public class TypeMappingProfile : Profile
    {
        public TypeMappingProfile()
        {
            CreateMap<User, ProfileViewModel>();
            CreateMap<UpdateProfileParameters, User>().ReverseMap();

            CreateMap<UserGroup, UserGroupViewModel>()
                .ForMember(t => t.Name, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(s => s.Group.IsActive))
                .ForMember(t => t.DateCreated, opt => opt.MapFrom(s => s.Group.DateCreated))
                .ForMember(t => t.RelationType, opt => opt.MapFrom(s => s.RelationType.ToString()));
        }
    }
}
