using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Najam.TaskBook.WebApi.Validation
{
    public class UnprocessableEntityResult : ObjectResult
    {
        private const int UnprocessableEntityStatusCode = 422;

        public UnprocessableEntityResult(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            StatusCode = UnprocessableEntityStatusCode;
        }
    }
}
