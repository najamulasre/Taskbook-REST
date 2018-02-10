using System.Collections.Generic;
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
    [Route("api/accounts/{username}/memberships")]
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
        public async Task<IActionResult> GetUserMemberships(string userName)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (loggedOnUser.Id != user.Id)
                return Forbid();

            UserGroup[] memberships = await _taskBookBusiness.GetUserMemberships(loggedOnUser.Id);

            var models = _mapper.Map<UserMembershipsViewModel[]>(memberships);

            return Ok(models);
        }
    }
}
