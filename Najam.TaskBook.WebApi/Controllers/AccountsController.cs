using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Najam.TaskBook.WebApi.Config;
using Najam.TaskBook.WebApi.Models;
using Najam.TaskBook.WebApi.Parameters.Accounts;
using IIdentityBusiness = Najam.TaskBook.WebApi.Business.IIdentityBusiness;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using User = Najam.TaskBook.WebApi.Data.Entities.User;


namespace Najam.TaskBook.WebApi.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        private readonly IIdentityBusiness _identityBusiness;
        private readonly JwtConfigOptions _jwtOptions;

        public AccountsController(
            IIdentityBusiness identityBusiness,
            IOptions<JwtConfigOptions> jwtOptionsSnapshot)
        {
            _identityBusiness = identityBusiness;
            _jwtOptions = jwtOptionsSnapshot.Value;
        }

        [HttpGet("{userName}", Name = nameof(GetAccount))]
        public async Task<IActionResult> GetAccount(string userName)
        {
            var user = await _identityBusiness.GetUserAsync(User);

            if (!string.Equals(userName, user.UserName, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            var account = new AccountViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(account);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountParameters parameters)
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

            IdentityResult result = await _identityBusiness.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {
                var account = new AccountViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                return CreatedAtRoute(nameof(GetAccount), new { parameters.UserName }, account);
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

            SignInResult result = await _identityBusiness.PasswordSignInAsync(parameters.UserName, parameters.Password);

            if (!result.Succeeded)
                return Unauthorized();

            var user = await _identityBusiness.FindByNameAsync(parameters.UserName);

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
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
