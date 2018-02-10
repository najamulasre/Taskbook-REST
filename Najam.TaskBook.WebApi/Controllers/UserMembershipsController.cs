using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.UserMemberships;

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
        public async Task<IActionResult> GetUserMemberships()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserGroup[] memberships = await _taskBookBusiness.GetUserMemberships(loggedOnUser.Id);

            var models = _mapper.Map<UserMembershipsViewModel[]>(memberships);

            return Ok(models);
        }
    }
}
