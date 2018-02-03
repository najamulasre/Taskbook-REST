﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Business;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Parameters.Credentials;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts/{userName}/credentials")]
    public class CredentialsController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;

        public CredentialsController(IIdentityBusiness identityBusiness)
        {
            _identityBusiness = identityBusiness;
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword(
            [FromRoute] string userName,
            [FromBody] ChangePasswordParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            User user = await _identityBusiness.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _identityBusiness.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            IdentityResult result = await _identityBusiness.ChangePasswordAsync(loggedOnUser, parameters.CurrentPassword, parameters.NewPassword);

            if (result.Succeeded)
                return NoContent();

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return UnprocessableEntity(ModelState);
        }
    }
}