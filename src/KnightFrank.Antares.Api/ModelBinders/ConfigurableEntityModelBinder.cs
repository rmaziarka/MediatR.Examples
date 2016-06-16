namespace KnightFrank.Antares.Api.ModelBinders
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using Newtonsoft.Json;

    public class ConfigurableActivityModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string requestContent = actionContext.Request.Content.ReadAsStringAsync().Result;
            var pageType = (PageType)actionContext.ActionArguments["pageType"];
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
