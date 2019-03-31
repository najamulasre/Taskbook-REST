using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.WebApi.Models.Tasks;
using Najam.TaskBook.WebApi.Parameters.TaskAssignments;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using ITaskBookBusiness = Najam.TaskBook.WebApi.Business.ITaskBookBusiness;
using Task = Najam.TaskBook.WebApi.Data.Entities.Task;
using User = Najam.TaskBook.WebApi.Data.Entities.User;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/TaskAssignments")]
    public class TaskAssignmentsController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;
        private readonly IMapper _mapper;

        public TaskAssignmentsController(IIdentityBusiness identityBusiness, ITaskBookBusiness taskBookBusiness, IMapper mapper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllUserTaskAssignments()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task[] userTasks = await _taskBookBusiness.GetUsersTaskAssignmentsByUserId(loggedOnUser.Id);

            var models = _mapper.Map<TaskViewModel[]>(userTasks);

            return Ok(models);
        }

        [HttpGet("{taskId}", Name = nameof(GetUserTaskAssignmentByTaskId))]
        [HttpHead("{taskId}")]
        public async Task<IActionResult> GetUserTaskAssignmentByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task assignedTask = await _taskBookBusiness.GetUsersTaskAssignmentByUserAndTaskId(loggedOnUser.Id, taskId);

            if (assignedTask == null)
                return NotFound();

            var model = _mapper.Map<TaskViewModel>(assignedTask);

            return Ok(model);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> CreateTaskAssignment(Guid? taskId)
        {
            if (!taskId.HasValue)
                return BadRequest();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task userTask = await _taskBookBusiness.GetUsersTaskByUserAndTaskId(loggedOnUser.Id, taskId.Value);

            if (userTask == null)
                return NotFound();

            if (userTask.AssignedToUserId.HasValue)
            {
                if (userTask.AssignedToUserId != loggedOnUser.Id)
                    return Conflict("Task has already been assigned to a different assignee.");

                if (userTask.DateTimeCompleted.HasValue)
                    return Conflict("Cannot create task assignment for a completed task.");

                Task existing = await _taskBookBusiness.GetUsersTaskAssignmentByUserAndTaskId(loggedOnUser.Id, taskId.Value);
                var existingModel = _mapper.Map<TaskViewModel>(existing);

                return Ok(existingModel);
            }

            Task assignedTask = await _taskBookBusiness.CreateTaskAssignmen(loggedOnUser.Id, taskId.Value);

            var model = _mapper.Map<TaskViewModel>(assignedTask);

            return CreatedAtRoute(nameof(GetUserTaskAssignmentByTaskId), new {TaskId = assignedTask.Id}, model);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAssignmentByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task assignedTask = await _taskBookBusiness.GetUsersTaskAssignmentByUserAndTaskId(loggedOnUser.Id, taskId);

            if (assignedTask?.AssignedToUserId == null)
                return NotFound();

            if (assignedTask.AssignedToUserId != loggedOnUser.Id)
                return Forbid();

            bool deleted = await _taskBookBusiness.DeleteTaskAssignment(taskId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET,HEAD,PUT,DELETE,OPTIONS");
            return NoContent();
        }
    }
}