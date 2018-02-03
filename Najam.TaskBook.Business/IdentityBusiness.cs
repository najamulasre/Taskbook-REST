using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Business
{
    public class IdentityBusiness : IIdentityBusiness
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityBusiness(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return _userManager.GetUserAsync(principal);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public Task<SignInResult> PasswordSignInAsync(string userName, string password)
        {
            return _signInManager.PasswordSignInAsync(userName, password, false, false);
        }

        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public Task<IdentityResult> UpdateAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }
    }
}
