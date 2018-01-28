using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Najam.TaskBook.Domain;
using Najam.TaskBook.WebApi.Config;
using Najam.TaskBook.WebApi.Models;
using Najam.TaskBook.WebApi.Parameters.Accounts;


namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtConfigOptions _jwtOptions;

        public AccountsController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtConfigOptions> jwtOptionsSnapshot)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptionsSnapshot.Value;
        }

        [HttpGet(Name = nameof(GetAccount))]
        public async Task<IActionResult> GetAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            var account = new AccountViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(account);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]CreateAccountParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var user = new User
            {
                UserName = parameters.UserName,
                Email = parameters.Email
            };

            var result = await _userManager.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {
                var account = new AccountViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                return CreatedAtRoute(nameof(GetAccount), account);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return UnprocessableEntity(ModelState);
        }

        [HttpPost("logon")]
        [AllowAnonymous]
        public async Task<IActionResult> Logon([FromBody] LogonParameters parameters)
        {
            if (parameters == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = await _signInManager.PasswordSignInAsync(parameters.UserName, parameters.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized();

            var user = _userManager.Users.Single(u => u.UserName == parameters.UserName);

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiresMinutes),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
