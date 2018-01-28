using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Models.Profiles;

namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts/{userName}/profile")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ProfilesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> GetProfile(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            User loggedOnUser = await _userManager.GetUserAsync(User);

            if (user.Id != loggedOnUser.Id)
                return Forbid();

            var profile = new ProfileViewModel
            {
                Email = loggedOnUser.Email,
                FirstName = loggedOnUser.FirstName,
                LastName = loggedOnUser.LastName,
                DateOfBirth = loggedOnUser.DateOfBirth
            };

            return Ok(profile);
        }
    }
}