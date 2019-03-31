using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.WebApi.Models.UserMemberships;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using ITaskBookBusiness = Najam.TaskBook.WebApi.Business.ITaskBookBusiness;
using User = Najam.TaskBook.WebApi.Data.Entities.User;
using UserGroup = Najam.TaskBook.WebApi.Data.Entities.UserGroup;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/memberships")]
    public class UserMembershipsController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;
        private readonly IMapper _mapper;

        public UserMembershipsController(
            IIdentityBusiness identityBusiness, 
            ITaskBookBusiness taskBookBusiness, 
            IMapper mapper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetUserMemberships()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserGroup[] memberships = await _taskBookBusiness.GetUserMemberships(loggedOnUser.Id);

            var models = _mapper.Map<UserMembershipsViewModel[]>(memberships);

            return Ok(models);
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET,HEAD,OPTIONS");
            return NoContent();
        }
    }
}
