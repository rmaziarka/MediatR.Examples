namespace KnightFrank.Antares.Api
{
    using System.Net.Http;
    using System.Web.Http.Filters;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using Ninject;

    /// <summary>
    /// Data shaping attribute.
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    public class DataShapingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the activity entity mapper.
        /// </summary>
        /// <value>
        /// The activity entity mapper.
        /// </value>
        [Inject]
        public IEntityMapper<Activity> ActivityEntityMapper { get; set; }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response?.Content as ObjectContent;
            if (objectContent == null)
            {
                return;
            }

            // TODO: move to dictionary method
            if (objectContent.ObjectType == typeof(Activity))
            {
                var activity = (Activity)objectContent.Value;
                this.ActivityEntityMapper.NullifyDisallowedValues(activity, PageType.Details);
            }

            if (objectContent.ObjectType == typeof(Offer))
            {
                var offer = (Offer)objectContent.Value;
                this.ActivityEntityMapper.NullifyDisallowedValues(offer.Activity, PageType.Details);
            }

            if (objectContent.ObjectType == typeof(Property))
            {
                var property = (Property)objectContent.Value;
                if (property.Activities != null)
                {
                    foreach (Activity activity in property.Activities)
                    {
                        this.ActivityEntityMapper.NullifyDisallowedValues(activity, PageType.Details);
                    }
                }
            }

        }
    }
}