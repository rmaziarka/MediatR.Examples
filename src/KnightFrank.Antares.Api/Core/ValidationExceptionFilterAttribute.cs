namespace KnightFrank.Antares.Api.Core
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    using FluentValidation;

    using Newtonsoft.Json;

    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
            {
                var validationException = (ValidationException)context.Exception;

                var response = new
                {
                    Message = "The request is invalid.",
                    Errors = validationException.Errors.Select(x => x.ErrorMessage)
                };
                
                context.Response = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), System.Text.Encoding.UTF8, "application/json"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}