using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Profiles;
using Najam.TaskBook.WebApi.Parameters.Profiles;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts/{userName}/profile")]
    public class ProfilesController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ProfilesController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetProfile(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _userManager.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            var profile = _mapper.Map<ProfileViewModel>(loggedOnUser);

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromRoute] string userName,
            [FromBody] UpdateProfileParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _userManager.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            _mapper.Map(parameters, loggedOnUser);

            IdentityResult result = await _userManager.UpdateAsync(loggedOnUser);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return UnprocessableEntity(ModelState);
            }

            User updatedUser = await _userManager.GetUserAsync(User);

            var profile = _mapper.Map<ProfileViewModel>(updatedUser);

            return Ok(profile);
        }

        [HttpPatch]
        public async Task<IActionResult> PartiallyUpdateProfile(
            [FromRoute] string userName, 
            [FromBody] JsonPatchDocument<UpdateProfileParameters> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _userManager.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            var userToPatch = _mapper.Map<UpdateProfileParameters>(loggedOnUser);

            patchDocument.ApplyTo(userToPatch, ModelState);

            TryValidateModel(userToPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(userToPatch, loggedOnUser);

            IdentityResult result = await _userManager.UpdateAsync(loggedOnUser);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return UnprocessableEntity(ModelState);
            }

            return NoContent();
        }
    }
}