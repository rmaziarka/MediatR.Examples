namespace KnightFrank.Antares.Api.ModelBinders
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Newtonsoft.Json;

    /// <summary>
    /// Configurable activity model binder
    /// </summary>
    /// <seealso cref="System.Web.Http.ModelBinding.IModelBinder" />
    public class ConfigurableRequirementModelBinder : IModelBinder
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
            if (actionContext.ActionArguments["pageType"] == null)
            {
                throw new ValidationException("Incorrect Page Type");
            }

            var pageType = (PageType)actionContext.ActionArguments["pageType"];
            switch (pageType)
            {
                case PageType.Create:
                    bindingContext.Model = JsonConvert.DeserializeObject<CreateRequirementCommand>(requestContent);
                    break;
                case PageType.Details:
                    bindingContext.Model = JsonConvert.DeserializeObject<Requirement>(requestContent);
                    break;
            }

            return true;
        }
    }
}
