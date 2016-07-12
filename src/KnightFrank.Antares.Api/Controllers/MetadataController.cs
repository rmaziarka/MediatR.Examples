namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    using KnightFrank.Antares.Api.ModelBinders;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;
    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

    /// <summary>
    /// Metadata controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/metadata")]
    public class MetadataController : ApiController
    {
        private readonly IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityConfiguration;
        private readonly IControlsConfiguration<Tuple<RequirementType>> requirementConfiguration;
        private readonly IControlsConfiguration<Tuple<OfferType, RequirementType>> offerConfiguration;
        private readonly IControlsConfiguration<Tuple<TenancyType, RequirementType>> tenancyTermConfiguration;

        private readonly IEnumParser enumParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataController"/> class.
        /// </summary>
        /// <param name="activityConfiguration">The activity configuration.</param>
        /// <param name="requirementConfiguration">The requirement configuration.</param>
        /// <param name="offerConfiguration">The offer configuration.</param>
        /// <param name="tenancyTermConfiguration">The tenancy configuration.</param>
        /// <param name="enumParser">The enum parser.</param>
        public MetadataController(
            IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityConfiguration,
            IControlsConfiguration<Tuple<OfferType, RequirementType>> offerConfiguration,
            IControlsConfiguration<Tuple<RequirementType>> requirementConfiguration,
            IControlsConfiguration<Tuple<TenancyType, RequirementType>> tenancyTermConfiguration,
            IEnumParser enumParser)
        {
            this.activityConfiguration = activityConfiguration;
            this.requirementConfiguration = requirementConfiguration;
            this.offerConfiguration = offerConfiguration;
            this.enumParser = enumParser;
            this.tenancyTermConfiguration = tenancyTermConfiguration;
        }

        /// <summary>
        /// Gets the activity configuration.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="propertyTypeId">Type of the property.</param>
        /// <param name="activityTypeId">Type of the activity.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("attributes/activity")]
        public dynamic GetActivityConfiguration(PageType pageType, Guid propertyTypeId, Guid activityTypeId, [ModelBinder(typeof(ConfigurableModelBinder<CreateActivityCommand, UpdateActivityCommand, Activity>))]object entity)
        {
            if (propertyTypeId == Guid.Empty || activityTypeId == Guid.Empty)
                return null;

            PropertyType propertyType = this.enumParser.Parse<Dal.Model.Property.PropertyType, PropertyType>(propertyTypeId);
            ActivityType activityType = this.enumParser.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(activityTypeId);
            IList<InnerFieldState> fieldStates = this.activityConfiguration.GetInnerFieldsState(pageType, new Tuple<PropertyType, ActivityType>(propertyType, activityType), entity);
            return fieldStates.MapToResponse();
        }

        /// <summary>
        /// Gets the requirement configuration.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="requirementTypeId">Type of the requirement.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("attributes/requirement")]
        public dynamic GetRequirementConfiguration(PageType pageType, Guid requirementTypeId, [ModelBinder(typeof(ConfigurableRequirementModelBinder))]object entity)
        {
            if (requirementTypeId == Guid.Empty)
            {
                return null;
            }

            RequirementType requirementType = this.enumParser.Parse<Dal.Model.Property.RequirementType, RequirementType>(requirementTypeId);
            IList<InnerFieldState> fieldStates = this.requirementConfiguration.GetInnerFieldsState(pageType, new Tuple<RequirementType>(requirementType), entity);
            return fieldStates.MapToResponse();
        }

        /// <summary>
        /// Gets the offer configuration.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="offerTypeId">Type of the offer.</param>
        /// <param name="requirementTypeId">Type of the requirement.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("attributes/offer")]
        public dynamic GetOfferConfiguration(PageType pageType, Guid offerTypeId, Guid requirementTypeId, [ModelBinder(typeof(ConfigurableModelBinder<CreateOfferCommand, UpdateOfferCommand, Offer>))]object entity)
        {
            if (offerTypeId == Guid.Empty || requirementTypeId == Guid.Empty)
                return null;

            OfferType offerType = this.enumParser.Parse<Dal.Model.Offer.OfferType, OfferType>(offerTypeId);
            RequirementType requirementType = this.enumParser.Parse<Dal.Model.Property.RequirementType, RequirementType>(requirementTypeId);

            IList<InnerFieldState> fieldStates = this.offerConfiguration.GetInnerFieldsState(pageType, new Tuple<OfferType, RequirementType>(offerType, requirementType), entity);
            return fieldStates.MapToResponse();
        }

        /// <summary>
        /// Gets the tenancy configuration.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="tenancyTypeId">Type of the tenancy.</param>
        /// <param name="requirementTypeId">Type of the requirement.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("attributes/tenancy")]
        public dynamic GetTenancyConfiguration(PageType pageType, Guid tenancyTypeId, Guid requirementTypeId, [ModelBinder(typeof(ConfigurableModelBinder<CreateTenancyCommand, UpdateTenancyCommand, Tenancy>))]object entity)
        {
            if (tenancyTypeId == Guid.Empty || requirementTypeId == Guid.Empty)
            {
                return null;
            }

            TenancyType tenancyType = this.enumParser.Parse<Dal.Model.Tenancy.TenancyType, TenancyType>(tenancyTypeId);
            RequirementType requirementType = this.enumParser.Parse<Dal.Model.Property.RequirementType, RequirementType>(requirementTypeId);

            IList<InnerFieldState> fieldStates = this.tenancyTermConfiguration.GetInnerFieldsState(pageType, new Tuple<TenancyType, RequirementType>(tenancyType, requirementType), entity);
            return fieldStates.MapToResponse();
        }
    }
}
