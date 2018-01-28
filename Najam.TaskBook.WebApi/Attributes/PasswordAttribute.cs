using System;
using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class PasswordAttribute : StringLengthAttribute
    {
        public PasswordAttribute() : base(32)
        {
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
            MinimumLength = 6;
        }
    }
}