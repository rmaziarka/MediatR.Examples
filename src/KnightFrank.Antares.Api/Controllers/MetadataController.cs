﻿namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    using KnightFrank.Antares.Api.ModelBinders;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    /// <summary>
    /// Metadata controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/metadata")]
    public class MetadataController : ApiController
    {
        private readonly IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityConfiguration;
        private readonly IControlsConfiguration<Tuple<RequirementType>> requirementConfiguration;
        private readonly IEnumParser enumParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataController"/> class.
        /// </summary>
        /// <param name="activityConfiguration">The activity configuration.</param>
        /// <param name="requirementConfiguration">The requirement configuration.</param>
        /// <param name="enumParser">The enum parser.</param>
        public MetadataController(IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityConfiguration, IControlsConfiguration<Tuple<RequirementType>> requirementConfiguration, IEnumParser enumParser)
        {
            this.activityConfiguration = activityConfiguration;
            this.requirementConfiguration = requirementConfiguration;
            this.enumParser = enumParser;
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
        public dynamic GetActivityConfiguration(PageType pageType, Guid propertyTypeId, Guid activityTypeId, [ModelBinder(typeof(ConfigurableActivityModelBinder))]object entity)
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
    }
}
