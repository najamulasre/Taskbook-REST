using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Business.Parameters;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Tasks;
using Task = Najam.TaskBook.Domain.Task;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    public class UserTasksController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly ITaskBookBusiness _taskBookBusiness;
        private readonly IMapper _mapper;

        public UserTasksController(IIdentityBusiness identityBusiness, ITaskBookBusiness taskBookBusiness, IMapper mapper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserTasks(GetUserTasksParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task[] userTasks = await _taskBookBusiness.GetUsersTaskByUserId(loggedOnUser.Id, parameters);

            var models = _mapper.Map<TaskViewModel[]>(userTasks);

            return Ok(models);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetUserTasksByTaskId(Guid taskId)
        {
            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            Task userTask = await _taskBookBusiness.GetUsersTaskByUserAndTaskId(loggedOnUser.Id, taskId);

            if (userTask == null)
                return NotFound();

            var model = _mapper.Map<TaskViewModel>(userTask);

            return Ok(model);
        }
    }
}