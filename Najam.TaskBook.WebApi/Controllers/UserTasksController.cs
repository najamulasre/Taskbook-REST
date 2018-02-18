using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Business.Dtos;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Tasks;
using Newtonsoft.Json;
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
        private readonly IUrlHelper _urlHelper;

        public UserTasksController(
            IIdentityBusiness identityBusiness, 
            ITaskBookBusiness taskBookBusiness,
            IMapper mapper, 
            IUrlHelper urlHelper)
        {
            _identityBusiness = identityBusiness;
            _taskBookBusiness = taskBookBusiness;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllUserTasks))]
        public async Task<IActionResult> GetAllUserTasks(GetUserTasksParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            UserTaskPage taskPage = await _taskBookBusiness.GetUsersTaskByUserId(loggedOnUser.Id, parameters);

            string previousPageLink = taskPage.HasPreviousPage
                ? _urlHelper.Link(nameof(GetAllUserTasks), new {PageSize = taskPage.PageSize, PageNumber = taskPage.CurrentPage - 1})
                : null;

            string nextPageLink = taskPage.HasNextPage
                ? _urlHelper.Link(nameof(GetAllUserTasks), new { PageSize = taskPage.PageSize, PageNumber = taskPage.CurrentPage + 1 })
                : null;

            var metaData = new
            {
                taskPage.TotalCount,
                taskPage.TotalPages,
                taskPage.CurrentPage,
                taskPage.PageSize,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-PagingMetadata", JsonConvert.SerializeObject(metaData));

            var models = _mapper.Map<TaskViewModel[]>(taskPage.Tasks);

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