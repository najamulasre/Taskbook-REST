using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Tasks;
using Najam.TaskBook.WebApi.Parameters.TaskAssignments;

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
        public async Task<IActionResult> GetAllUserTaskAssignmentss()
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Domain.Task[] userTasks = await _taskBookBusiness.GetUsersTaskAssignmentsByUserId(loggedOnUser.Id);

            var models = _mapper.Map<TaskViewModel[]>(userTasks);

            return Ok(models);
        }

        [HttpGet("{taskId}", Name = nameof(GetUserTaskAssignmentByTaskId))]
        public async Task<IActionResult> GetUserTaskAssignmentByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Domain.Task assignedTask = await _taskBookBusiness.GetUsersTaskAssignmentByUserAndTaskId(loggedOnUser.Id, taskId);

            if (assignedTask == null)
                return NotFound();

            var model = _mapper.Map<TaskViewModel>(assignedTask);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAssignment([FromBody] CreateTaskAssignmentParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid || !parameters.TaskId.HasValue)
                return UnprocessableEntity(ModelState);

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Domain.Task userTask = await _taskBookBusiness.GetUsersTaskByUserAndTaskId(loggedOnUser.Id, parameters.TaskId.Value);

            if (userTask == null)
                return NotFound();

            if (userTask.AssignedToUserId.HasValue)
                return Conflict("Task has already been assigned.");

            Domain.Task assignedTask = await _taskBookBusiness.AssignTask(loggedOnUser.Id, parameters.TaskId.Value);

            var model = _mapper.Map<TaskViewModel>(assignedTask);

            return CreatedAtRoute(nameof(GetUserTaskAssignmentByTaskId), new {TaskId = assignedTask.Id}, model);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAssignmentByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Domain.Task assignedTask = await _taskBookBusiness.GetUsersTaskAssignmentByUserAndTaskId(loggedOnUser.Id, taskId);

            if (assignedTask?.AssignedToUserId == null)
                return NotFound();

            if (assignedTask.AssignedToUserId != loggedOnUser.Id)
                return Forbid();

            bool deleted = await _taskBookBusiness.UnassignTask(taskId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}