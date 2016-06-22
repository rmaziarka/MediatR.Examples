namespace KnightFrank.Antares.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Filters;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using Ninject;
    using Ninject.Web.WebApi.Filter;

    /// <summary>
    /// Data shaping filter
    /// </summary>
    /// <seealso cref="Ninject.Web.WebApi.Filter.AbstractActionFilter" />
    public class DataShapingFilter : AbstractActionFilter
    {
        /// <summary>
        /// Gets or sets the activity entity mapper.
        /// </summary>
        /// <value>
        /// The activity entity mapper.
        /// </value>
        [Inject]
        private readonly IEntityMapper<Activity> activityEntityMapper;

        private readonly IDictionary<Type, Action<ObjectContent>> shapingFunctions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataShapingFilter"/> class.
        /// </summary>
        public DataShapingFilter(IEntityMapper<Activity> activityEntityMapper)
        {
            this.activityEntityMapper = activityEntityMapper;
            this.shapingFunctions = new Dictionary<Type, Action<ObjectContent>>
            {
                { typeof(Activity), this.ShapeActivity },
                { typeof(Offer), this.ShapeOffer },
                { typeof(Property), this.ShapeProperty }
            };
        }

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

            if (this.shapingFunctions.ContainsKey(objectContent.ObjectType))
            {
                this.shapingFunctions[objectContent.ObjectType].Invoke(objectContent);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this filter can occur multiple times.
        /// </summary>
        /// <value>
        /// True if the filter can occur multiple times, False otherwise.
        /// </value>
        public override bool AllowMultiple => true;

        private void ShapeActivity(ObjectContent objectContent)
        {
            var activity = (Activity)objectContent.Value;
            this.activityEntityMapper.NullifyDisallowedValues(activity, PageType.Details);
        }

        private void ShapeOffer(ObjectContent objectContent)
        {
            var offer = (Offer)objectContent.Value;
            this.activityEntityMapper.NullifyDisallowedValues(offer.Activity, PageType.Details);
        }

        private void ShapeProperty(ObjectContent objectContent)
        {
            var property = (Property)objectContent.Value;
            if (property.Activities != null)
            {
                foreach (Activity activity in property.Activities)
                {
                    this.activityEntityMapper.NullifyDisallowedValues(activity, PageType.Details);
                }
            }
        }
    }
}