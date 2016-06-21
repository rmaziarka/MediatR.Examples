namespace KnightFrank.Antares.Api.ModelBinders
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Configurable activity model binder
    /// </summary>
    /// <seealso cref="System.Web.Http.ModelBinding.IModelBinder" />
    public class ConfigurableActivityModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// true if model binding is successful; otherwise, false.
        /// </returns>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string requestContent = actionContext.Request.Content.ReadAsStringAsync().Result;
            PageType pageType;
            if (!EnumExtensions.TryParseEnum((string)actionContext.ActionArguments["pageType"], out pageType))
            {
                throw new ValidationException("Incorrect Page Type");
            }

            switch (pageType)
            {
                case PageType.Create:
                    bindingContext.Model = JsonConvert.DeserializeObject<CreateActivityCommand>(requestContent);
                    break;
                case PageType.Update:
                    bindingContext.Model = JsonConvert.DeserializeObject<UpdateActivityCommand>(requestContent);
                    break;
                case PageType.Details:
                case PageType.Preview:
                    bindingContext.Model = JsonConvert.DeserializeObject<Activity>(requestContent);
                    break;
            }
            return true;
        }
    }
}
