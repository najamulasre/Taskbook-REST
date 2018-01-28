using System.Threading.Tasks;
using AutoMapper;
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
    }
}