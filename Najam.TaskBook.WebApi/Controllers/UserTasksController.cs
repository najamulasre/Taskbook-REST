using System;
using System.Collections.Generic;
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

            string previousPageLink = CreatePageLink(taskPage, parameters, -1);
            string nextPageLink = CreatePageLink(taskPage, parameters, 1);

            var metaData = new
            {
                taskPage.TotalCount,
                taskPage.PageSize,
                taskPage.TotalPages,
                taskPage.CurrentPage,
                previousPageLink,
                nextPageLink
            };

            string pagingMetaDataJson = JsonConvert.SerializeObject(metaData);

            Response.Headers.Add("X-PagingMetadata", pagingMetaDataJson);

            var models = _mapper.Map<TaskViewModel[]>(taskPage.Tasks);

            return Ok(models);
        }

        private string CreatePageLink(UserTaskPage taskPage, GetUserTasksParameters parameters, int offset)
        {
            if (offset > 0 && !taskPage.HasNextPage)
                return null;

            if (offset < 0 && !taskPage.HasPreviousPage)
                return null;

            var routeValues = new
            {
                PageSize = taskPage.PageSize,
                PageNumber = taskPage.CurrentPage + offset,
                SearchQuery = parameters.SearchQuery
            };

            string link = _urlHelper.Link(nameof(GetAllUserTasks), routeValues);

            return link;
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