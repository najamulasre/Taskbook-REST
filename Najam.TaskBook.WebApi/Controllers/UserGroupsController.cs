using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.UserGroups;
using Najam.TaskBook.WebApi.Parameters.UserGroups;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts/{username}/groups")]
    public class UserGroupsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;

        public UserGroupsController(
            IMapper mapper,
            IIdentityBusiness identityBusiness, 
            ITaskBookBusiness taskBookBusiness)
        {
            _mapper = mapper;
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUserGroups(string userName)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            UserGroup[] userGroups = await _taskBookBusiness.GetUserGroupsByUserId(loggedOnUser.Id);

            var viewModels = _mapper.Map<UserGroupViewModel[]>(userGroups);

            return Ok(viewModels);
        }

        [HttpGet("{groupId}", Name = nameof(GetUserGroupById))]
        public async Task<IActionResult> GetUserGroupById(string userName, Guid groupId)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            UserGroup userGroup = await _taskBookBusiness.GetUserGroupByGroupId(loggedOnUser.Id, groupId);

            if (userGroup == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<UserGroupViewModel>(userGroup);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserGroup(string userName, [FromBody]CreateUserGroupParameters parameters)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            UserGroup createdGroup = await _taskBookBusiness.CreateUserGroup(loggedOnUser.Id, parameters.GroupName, parameters.IsActive);

            var viewModel = _mapper.Map<UserGroupViewModel>(createdGroup);

            return CreatedAtRoute(nameof(GetUserGroupById), new {viewModel.GroupId}, viewModel);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateUserGroup(string userName, Guid groupId, [FromBody]UpdateUserGroupParameters parameters)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            UserGroup groupToUpdateGroup = await _taskBookBusiness.GetUserGroupByGroupId(loggedOnUser.Id, groupId);

            if (groupToUpdateGroup == null)
                return NotFound();

            if (groupToUpdateGroup.RelationType != UserGroupRelationType.Owner)
                return Forbid();

            UserGroup updatedGroup = await _taskBookBusiness.UpdateGroup(loggedOnUser.Id, groupId, parameters.GroupName, parameters.IsActive);

            var viewModel = _mapper.Map<UserGroupViewModel>(updatedGroup);

            return Ok(viewModel);
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteUserGroup(string userName, Guid groupId)
        {
            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            UserGroup groupToDelete = await _taskBookBusiness.GetUserGroupByGroupId(loggedOnUser.Id, groupId);

            if (groupToDelete == null)
                return NotFound();

            if (groupToDelete.RelationType != UserGroupRelationType.Owner)
                return Forbid();

            await _taskBookBusiness.DeleteGroup(groupId);

            return NoContent();
        }
    }
}