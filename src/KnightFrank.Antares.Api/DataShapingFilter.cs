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
    using KnightFrank.Antares.Domain.Common;

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
        private IEntityMapper<Activity> activityEntityMapper;

        /// <summary>
        /// Gets or sets the offer entity mapper.
        /// </summary>
        /// <value>
        /// The offer entity mapper.
        /// </value>
        private IEntityMapper<Offer> offerEntityMapper;

        private readonly INinjectInstanceResolver ninjectInstanceResolver;

        private readonly IDictionary<Type, Action<ObjectContent>> shapingFunctions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataShapingFilter"/> class.
        /// </summary>
        public DataShapingFilter(INinjectInstanceResolver ninjectInstanceResolver)
        {
            this.ninjectInstanceResolver = ninjectInstanceResolver;
            this.shapingFunctions = new Dictionary<Type, Action<ObjectContent>>
            {
                { typeof(Activity), this.ShapeActivity },
                { typeof(Offer), this.ShapeOffer },
                { typeof(Property), this.ShapeProperty }
            };
        }

        private void InitDependencies()
        {
            this.activityEntityMapper = this.ninjectInstanceResolver.GetInstance<IEntityMapper<Activity>>();
            this.offerEntityMapper = this.ninjectInstanceResolver.GetInstance<IEntityMapper<Offer>>();
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

            this.InitDependencies();

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
            if (activity.Offers != null)
            {
                foreach (Offer offer in activity.Offers)
                {
                    // TODO don't shape activity associated with offer, because it is not fetched from DB
                    // think of global solution
                    this.offerEntityMapper.NullifyDisallowedValues(offer, PageType.Details);
                }
            }
        }

        private void ShapeOffer(ObjectContent objectContent)
        {
            var offer = (Offer)objectContent.Value;
            this.offerEntityMapper.NullifyDisallowedValues(offer, PageType.Details);
            this.activityEntityMapper.NullifyDisallowedValues(offer.Activity, PageType.Details);
            //TODO add NullifyDisallowedValues for requirement
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