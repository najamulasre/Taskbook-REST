using AutoMapper;
using Najam.TaskBook.WebApi.Models.GroupMemberships;
using Najam.TaskBook.WebApi.Models.Profiles;
using Najam.TaskBook.WebApi.Models.Tasks;
using Najam.TaskBook.WebApi.Models.UserGroups;
using Najam.TaskBook.WebApi.Models.UserMemberships;
using Najam.TaskBook.WebApi.Parameters.Profiles;
using Task = Najam.TaskBook.WebApi.Data.Entities.Task;
using UserGroup = Najam.TaskBook.WebApi.Data.Entities.UserGroup;

namespace Najam.TaskBook.WebApi.Mappings
{
    public class TypeMappingProfile : Profile
    {
        public TypeMappingProfile()
        {
            CreateMap<Data.Entities.User, ProfileViewModel>();
            CreateMap<UpdateProfileParameters, Data.Entities.User>().ReverseMap();

            CreateMap<UserGroup, UserGroupViewModel>()
                .ForMember(t => t.Name, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(s => s.Group.IsActive))
                .ForMember(t => t.DateCreated, opt => opt.MapFrom(s => s.Group.DateCreated))
                .ForMember(t => t.RelationType, opt => opt.MapFrom(s => s.RelationType.ToString()));

            CreateMap<UserGroup, GroupMembershipViewModel>()
                .ForMember(t => t.UserName, opt => opt.MapFrom(s => s.User.UserName))
                .ForMember(t => t.GroupName, opt => opt.MapFrom(s => s.Group.Name));

            CreateMap<UserGroup, UserMembershipsViewModel>()
                .ForMember(t => t.UserName, opt => opt.MapFrom(s => s.User.UserName.ToUpper()))
                .ForMember(t => t.GroupId, opt => opt.MapFrom(s => s.Group.Id))
                .ForMember(t => t.GroupName, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(t => t.IsGroupActive, opt => opt.MapFrom(s => s.Group.IsActive))
                .ForMember(t => t.MembershipType, opt => opt.MapFrom(s => s.RelationType.ToString()));

            CreateMap<Task, TaskViewModel>()
                .ForMember(t => t.GroupName, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(t => t.CreatedBy, opt => opt.MapFrom(s => s.CreatedByUser.UserName))
                .ForMember(t => t.AssignedTo, opt => opt.MapFrom(s => s.AssignedToUser == null ? null : s.AssignedToUser.UserName));
        }
    }
}
