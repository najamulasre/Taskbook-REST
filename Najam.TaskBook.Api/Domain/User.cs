using System;
using Microsoft.AspNetCore.Identity;

namespace Najam.TaskBook.Api.Domain
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser<Guid>
    {
    }

    public class Role : IdentityRole<Guid>
    {
    }
}
