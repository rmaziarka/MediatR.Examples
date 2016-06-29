namespace KnightFrank.Antares.Api.ModelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using FluentValidation;

    using KnightFrank.Antares.Api.Core;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
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
            if (actionContext.ActionArguments["pageType"] == null)
            {
                throw new ValidationException("Incorrect Page Type");
            }

            var jsonSerializerSettings = new JsonSerializerSettings
                                             {
                                                 Converters = new List<JsonConverter> { new NullToDefaultValueConverter<Guid>() }
                                             };

            var pageType = (PageType)actionContext.ActionArguments["pageType"];
            switch (pageType)
            {
                case PageType.Create:
                    bindingContext.Model = JsonConvert.DeserializeObject<CreateActivityCommand>(requestContent, jsonSerializerSettings);
                    break;
                case PageType.Update:
                    bindingContext.Model = JsonConvert.DeserializeObject<UpdateActivityCommand>(requestContent, jsonSerializerSettings);
                    break;
                case PageType.Details:
                case PageType.Preview:
                    bindingContext.Model = JsonConvert.DeserializeObject<Activity>(requestContent, jsonSerializerSettings);
                    break;
            }
            return true;
        }
    }
}
