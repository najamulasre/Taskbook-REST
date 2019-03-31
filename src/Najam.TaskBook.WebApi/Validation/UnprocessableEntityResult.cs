using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Najam.TaskBook.WebApi.Validation
{
    public class UnprocessableEntityResult : ObjectResult
    {
        public UnprocessableEntityResult(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
