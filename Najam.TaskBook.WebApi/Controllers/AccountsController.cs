using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Parameters.Accounts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Najam.TaskBook.WebApi.Controllers
{
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateAccountParameters parameters)
        {
            var user = new User
            {
                UserName = parameters.UserName,
            };

            var result = await _userManager.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
