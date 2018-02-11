using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.GroupTasks;
using Najam.TaskBook.WebApi.Parameters.GroupTasks;
using Task = Najam.TaskBook.Domain.Task;

namespace Najam.TaskBook.WebApi.Controllers
{
    /* All member can have access to all actions */
    [Authorize]
    [Route("api/groups/{groupId}/tasks")]
    public class GroupTasksController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;
        private readonly IMapper _mapper;

        public GroupTasksController(
            IIdentityBusiness identityBusiness,
            ITaskBookBusiness taskBookBusiness,
            IMapper mapper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroupTaks(Guid groupId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            bool isUserRelatedWithGroup = await _taskBookBusiness.IsUserRelatedWithGroup(loggedOnUser.Id, groupId);

            if (!isUserRelatedWithGroup)
                return NotFound();

            Task[] tasks = await _taskBookBusiness.GetTasksByGroupId(groupId);

            var models = _mapper.Map<TaskViewModel[]>(tasks);

            return Ok(models);
        }

        [HttpGet("{taskId}", Name = nameof(GetTaskByTaskId))]
        public async Task<IActionResult> GetTaskByTaskId(Guid groupId, Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            bool isUserRelatedWithGroup = await _taskBookBusiness.IsUserRelatedWithGroup(loggedOnUser.Id, groupId);

            if (!isUserRelatedWithGroup)
                return NotFound();

            Task task = await _taskBookBusiness.GetTaskByTaskId(taskId);

            if (task == null)
                return NotFound();

            var models = _mapper.Map<TaskViewModel>(task);

            return Ok(models);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(Guid groupId, [FromBody] CreateGroupTaskParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            bool isUserRelatedWithGroup = await _taskBookBusiness.IsUserRelatedWithGroup(loggedOnUser.Id, groupId);

            if (!isUserRelatedWithGroup)
                return NotFound();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            if (!parameters.Deadline.HasValue)
            {
                ModelState.AddModelError(nameof(parameters.Deadline), "Please provide a deadline date.");
                return UnprocessableEntity(ModelState);
            }

            DateTime serverDateTime = await _taskBookBusiness.GetServerDateTime();

            if (parameters.Deadline < serverDateTime)
            {
                ModelState.AddModelError(nameof(parameters.Deadline), "Deadline cannot be in the past.");
                return UnprocessableEntity(ModelState);
            }

            Task createdTask = await _taskBookBusiness.CreateGroupTask(
                groupId,
                parameters.Title,
                parameters.Description,
                parameters.Deadline.Value,
                loggedOnUser.Id);

            var model = _mapper.Map<TaskViewModel>(createdTask);

            return CreatedAtRoute(nameof(GetTaskByTaskId), new {GroupId = createdTask.GroupId, TaskId = createdTask.Id}, model);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult> CreateTask(Guid groupId, Guid taskId, [FromBody] UpdateGroupTaskParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            bool isUserRelatedWithGroup = await _taskBookBusiness.IsUserRelatedWithGroup(loggedOnUser.Id, groupId);

            if (!isUserRelatedWithGroup)
                return NotFound();

            bool isUserTaskCreator = await _taskBookBusiness.IsUserTaskCreator(loggedOnUser.Id, taskId);

            if (!isUserTaskCreator)
                return Forbid();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            if (!parameters.Deadline.HasValue)
            {
                ModelState.AddModelError(nameof(parameters.Deadline), "Please provide a deadline date.");
                return UnprocessableEntity(ModelState);
            }

            DateTime serverDateTime = await _taskBookBusiness.GetServerDateTime();

            if (parameters.Deadline < serverDateTime)
            {
                ModelState.AddModelError(nameof(parameters.Deadline), "Deadline cannot be in the past.");
                return UnprocessableEntity(ModelState);
            }

            Task updatedTask = await _taskBookBusiness.UpdateGroupTask(
                taskId,
                parameters.Title,
                parameters.Description,
                parameters.Deadline.Value);

            if (updatedTask == null)
                return NotFound();

            var model = _mapper.Map<TaskViewModel>(updatedTask);

            return Ok(model);
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid groupId, Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            bool isUserRelatedWithGroup = await _taskBookBusiness.IsUserRelatedWithGroup(loggedOnUser.Id, groupId);

            if (!isUserRelatedWithGroup)
                return NotFound();

            Task task = await _taskBookBusiness.GetTaskByTaskId(taskId);

            if (task == null)
                return NotFound();

            bool isUserTaskCreator = await _taskBookBusiness.IsUserTaskCreator(loggedOnUser.Id, taskId);

            if (!isUserTaskCreator)
                return Forbid();

            bool deleted = await _taskBookBusiness.DeleteTask(taskId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}