namespace KnightFrank.Antares.Api.Core
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Filters;

    using FluentValidation;

    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using Newtonsoft.Json;

    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Exception exception = context.Exception;

            if (exception is ValidationException)
            {
                var validationException = exception as ValidationException;

                var response = validationException.Errors.Select(x => new { message = x.ErrorMessage, code = x.ErrorCode });
                context.Response = new HttpResponseMessage
                {
                    Content = CreateContent(response),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (exception is BusinessValidationException)
            {
                var validationException = exception as BusinessValidationException;

                var response = new
                {
                    Message = "The request is invalid. " + validationException.Message,
                    Errors = new { message = exception.Message }
                };

                context.Response = new HttpResponseMessage
                {
                    Content = CreateContent(response),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        private static StringContent CreateContent(object response)
        {
            return new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
        }
    }
}