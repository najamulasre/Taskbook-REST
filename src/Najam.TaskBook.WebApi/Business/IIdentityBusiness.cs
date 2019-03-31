using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Business
{
    public interface IIdentityBusiness
    {
        Task<User> GetUserAsync(ClaimsPrincipal principal);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<User> FindByNameAsync(string userName);

        Task<SignInResult> PasswordSignInAsync(string userName, string password);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<IdentityResult> UpdateAsync(User user);
    }
}