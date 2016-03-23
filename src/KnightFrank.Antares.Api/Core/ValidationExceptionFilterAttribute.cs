namespace KnightFrank.Antares.Api.Core
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Filters;

    using FluentValidation;

    using KnightFrank.Antares.Domain.Common.Exceptions;

    using Newtonsoft.Json;

    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // TODO refactor
            if (context.Exception is ValidationException)
            {
                var validationException = (ValidationException)context.Exception;

                var response = new
                {
                    Message = "The request is invalid.",
                    Errors = validationException.Errors.Select(x => x.ErrorMessage),
                    InvalidFields = validationException.Errors.Select(x => x.PropertyName)
                };

                context.Response = new HttpResponseMessage
                {
                    Content = CreateContent(response),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (context.Exception is DomainValidationException)
            {
                var response =
                    new
                    {
                        Message = "Domain validation occured. Field is invalid.",
                        InvalidFields = context.Exception.Message
                    };

                context.Response = new HttpResponseMessage
                {
                    Content = CreateContent(response),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (context.Exception is ResourceNotFoundException)
            {
                var resourceNotFoundException = (ResourceNotFoundException)context.Exception;

                var response =
                    new
                    {
                        Message = resourceNotFoundException.Message,
                        ResourceId = resourceNotFoundException.ResourceId,
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