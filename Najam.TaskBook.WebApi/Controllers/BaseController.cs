using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Najam.TaskBook.WebApi.Validation;

namespace Najam.TaskBook.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected UnprocessableEntityResult UnprocessableEntity(ModelStateDictionary modelState) => new UnprocessableEntityResult(modelState);

        protected ConflictResult Conflict(string message) => new ConflictResult(message);
    }
}
