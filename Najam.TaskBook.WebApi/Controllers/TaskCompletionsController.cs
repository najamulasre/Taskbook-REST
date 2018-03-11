using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.WebApi.Models.Tasks;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using ITaskBookBusiness = Najam.TaskBook.WebApi.Business.ITaskBookBusiness;
using Task = Najam.TaskBook.WebApi.Data.Entities.Task;
using User = Najam.TaskBook.WebApi.Data.Entities.User;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/TaskCompletions")]
    public class TaskCompletionsController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;
        private readonly IMapper _mapper;

        public TaskCompletionsController(IIdentityBusiness identityBusiness, ITaskBookBusiness taskBookBusiness, IMapper mapper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserTaskCompletions()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task[] userTasks = await _taskBookBusiness.GetUsersTaskCompletionsByUserId(loggedOnUser.Id);

            var models = _mapper.Map<TaskViewModel[]>(userTasks);

            return Ok(models);
        }

        [HttpGet("{taskId}", Name = nameof(GetUserTaskCompletionByTaskId))]
        public async Task<IActionResult> GetUserTaskCompletionByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task completedTask = await _taskBookBusiness.GetUsersTaskCompletionByUserAndTaskId(loggedOnUser.Id, taskId);

            if (completedTask == null)
                return NotFound();

            var model = _mapper.Map<TaskViewModel>(completedTask);

            return Ok(model);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> CreateTaskCompletion(Guid? taskId)
        {
            if (!taskId.HasValue)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task userTask = await _taskBookBusiness.GetUsersTaskByUserAndTaskId(loggedOnUser.Id, taskId.Value);

            if (userTask == null)
                return NotFound();

            if (!userTask.AssignedToUserId.HasValue)
                return Conflict("There should be a task assignment before task completion.");

            if (userTask.AssignedToUserId != loggedOnUser.Id)
                return Conflict("Assignment to another assignee exists.");

            if (userTask.DateTimeCompleted.HasValue)
            {
                Task existing = await _taskBookBusiness.GetUsersTaskCompletionByUserAndTaskId(loggedOnUser.Id, taskId.Value);
                var existingModel = _mapper.Map<TaskViewModel>(existing);
                return Ok(existingModel);
            }

            Task completedTask = await _taskBookBusiness.CreateTaskCompletion(loggedOnUser.Id, taskId.Value);

            var model = _mapper.Map<TaskViewModel>(completedTask);

            return CreatedAtRoute(nameof(GetUserTaskCompletionByTaskId), new { TaskId = completedTask.Id }, model);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskCompletionByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task completedTask = await _taskBookBusiness.GetUsersTaskCompletionByUserAndTaskId(loggedOnUser.Id, taskId);

            if (completedTask?.DateTimeCompleted == null || completedTask.AssignedToUserId == null)
                return NotFound();

            if (completedTask.AssignedToUserId != loggedOnUser.Id)
                return Forbid();

            bool deleted = await _taskBookBusiness.DeleteTaskCompletion(taskId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}