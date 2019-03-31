using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Najam.TaskBook.WebApi.Validation
{
    public class ConflictResult : ObjectResult
    {
        public ConflictResult(string message = "Conflict") : base(new {Reason = message})
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}