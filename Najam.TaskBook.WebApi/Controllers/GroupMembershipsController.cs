using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.WebApi.Models.GroupMemberships;
using Najam.TaskBook.WebApi.Parameters.GroupMemberships;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using ITaskBookBusiness = Najam.TaskBook.WebApi.Business.ITaskBookBusiness;
using User = Najam.TaskBook.WebApi.Data.Entities.User;
using UserGroup = Najam.TaskBook.WebApi.Data.Entities.UserGroup;
using UserGroupRelationType = Najam.TaskBook.WebApi.Data.Entities.UserGroupRelationType;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/groups/{groupId}/memberships")]
    public class GroupMembershipsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;

        public GroupMembershipsController(IMapper mapper, IIdentityBusiness identityBusiness, ITaskBookBusiness taskBookBusiness)
        {
            _mapper = mapper;
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
        }

        [HttpGet("{memberUserName}", Name = nameof(GetMembershipByMemberUserName))]
        public async Task<IActionResult> GetMembershipByMemberUserName(Guid groupId, string memberUserName)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            var loggedOnUserIsGroupOwner = await _taskBookBusiness.IsUserGroupOwner(loggedOnUser.Id, groupId);

            if (!loggedOnUserIsGroupOwner)
                return NotFound();

            User memberUser = await _identityBusiness.FindByNameAsync(memberUserName);

            if (memberUser == null)
                return NotFound();

            UserGroup membership = await _taskBookBusiness.GetGroupMembership(memberUser.Id, groupId);

            if (membership == null)
                return NotFound();

            var viewModel = _mapper.Map<GroupMembershipViewModel>(membership);

            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMemberships(Guid groupId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            var loggedOnUserIsGroupOwner = await _taskBookBusiness.IsUserGroupOwner(loggedOnUser.Id, groupId);

            if (!loggedOnUserIsGroupOwner)
                return NotFound();

            UserGroup[] memberships = await _taskBookBusiness.GetGroupMemberships(groupId);

            var viewModels = _mapper.Map<GroupMembershipViewModel[]>(memberships);

            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMembership(Guid groupId, [FromBody] CreateGroupMembershipParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            var loggedOnUserIsGroupOwner = await _taskBookBusiness.IsUserGroupOwner(loggedOnUser.Id, groupId);

            if (!loggedOnUserIsGroupOwner)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            User memberUser = await _identityBusiness.FindByNameAsync(parameters.UserName);

            if (memberUser == null)
                return NotFound();

            UserGroup existingMembership = await _taskBookBusiness.GetGroupMembership(memberUser.Id, groupId);

            if (existingMembership != null)
                return Conflict($"'{memberUser.UserName}' is already member of '{existingMembership.Group.Name}'.");

            UserGroup membership = await _taskBookBusiness.CrateGroupMembership(memberUser.Id, groupId);

            var viewModel = _mapper.Map<GroupMembershipViewModel>(membership);

            return CreatedAtRoute(nameof(GetMembershipByMemberUserName), new { memberUserName = memberUser.UserName}, viewModel);
        }

        [HttpDelete("{memberUserName}")]
        public async Task<IActionResult> DeleteMembership(Guid groupId, string memberUserName)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            var loggedOnUserIsGroupOwner = await _taskBookBusiness.IsUserGroupOwner(loggedOnUser.Id, groupId);

            if (!loggedOnUserIsGroupOwner)
                return NotFound();

            User memberUser = await _identityBusiness.FindByNameAsync(memberUserName);

            if (memberUser == null)
                return NotFound();

            UserGroup membership = await _taskBookBusiness.GetGroupMembership(memberUser.Id, groupId);

            if (membership == null)
                return NotFound();

            if (membership.RelationType != UserGroupRelationType.Member)
                return Forbid();

            await _taskBookBusiness.DeleteGroupMembership(memberUser.Id, groupId);

            return NoContent();
        }
    }
}