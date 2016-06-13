namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove;

    using Newtonsoft.Json;

    public class ConfigurableActivityModelBinder:IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string requestContent = actionContext.Request.Content.ReadAsStringAsync().Result;
            var pageType = (PageType)actionContext.ActionArguments["pageType"];
            switch (pageType)
            {
                case PageType.Create:
                    bindingContext.Model = JsonConvert.DeserializeObject<CreateCommand>(requestContent);
                    break;
                case PageType.Update:
                    bindingContext.Model = JsonConvert.DeserializeObject<UpdateCommand>(requestContent);
                    break;
                case PageType.Details:
                    bindingContext.Model = JsonConvert.DeserializeObject<IActivity>(requestContent);
                    break;
            }
            return true;
        }
    }
}
