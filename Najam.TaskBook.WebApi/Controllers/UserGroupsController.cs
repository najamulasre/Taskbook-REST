using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.WebApi.Models.UserGroups;
using Najam.TaskBook.WebApi.Parameters.UserGroups;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using ITaskBookBusiness = Najam.TaskBook.WebApi.Business.ITaskBookBusiness;
using User = Najam.TaskBook.WebApi.Data.Entities.User;
using UserGroup = Najam.TaskBook.WebApi.Data.Entities.UserGroup;
using UserGroupRelationType = Najam.TaskBook.WebApi.Data.Entities.UserGroupRelationType;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/groups")]
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
        [HttpHead]
        public async Task<IActionResult> GetAllUserGroups()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserGroup[] userGroups = await _taskBookBusiness.GetUserGroupsByUserId(loggedOnUser.Id);

            var viewModels = _mapper.Map<UserGroupViewModel[]>(userGroups);

            return Ok(viewModels);
        }

        [HttpGet("{groupId}", Name = nameof(GetUserGroupById))]
        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetUserGroupById(Guid groupId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserGroup userGroup = await _taskBookBusiness.GetUserGroupByGroupId(loggedOnUser.Id, groupId);

            if (userGroup == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<UserGroupViewModel>(userGroup);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserGroup([FromBody]CreateUserGroupParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            UserGroup createdGroup = await _taskBookBusiness.CreateUserGroup(loggedOnUser.Id, parameters.GroupName, parameters.IsActive);

            var viewModel = _mapper.Map<UserGroupViewModel>(createdGroup);

            return CreatedAtRoute(nameof(GetUserGroupById), new {viewModel.GroupId}, viewModel);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateUserGroup(Guid groupId, [FromBody]UpdateUserGroupParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

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
        public async Task<IActionResult> DeleteUserGroup(Guid groupId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserGroup groupToDelete = await _taskBookBusiness.GetUserGroupByGroupId(loggedOnUser.Id, groupId);

            if (groupToDelete == null)
                return NotFound();

            if (groupToDelete.RelationType != UserGroupRelationType.Owner)
                return Forbid();

            await _taskBookBusiness.DeleteGroup(groupId);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,PUT,DELETE,OPTIONS");
            return NoContent();
        }
    }
}