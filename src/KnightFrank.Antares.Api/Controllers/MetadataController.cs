namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
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
        private readonly IControlsConfiguration<PropertyType, ActivityType> activityConfiguration;
        private readonly IEnumParser enumParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataController"/> class.
        /// </summary>
        /// <param name="activityConfiguration">The activity configuration.</param>
        /// <param name="enumParser">The enum parser.</param>
        public MetadataController(IControlsConfiguration<PropertyType, ActivityType> activityConfiguration, IEnumParser enumParser)
        {
            this.activityConfiguration = activityConfiguration;
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
            PropertyType propertyType = this.enumParser.Parse<Dal.Model.Property.PropertyType, PropertyType>(propertyTypeId);
            ActivityType activityType = this.enumParser.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(activityTypeId);
            IList<InnerFieldState> fieldStates = this.activityConfiguration.GetInnerFieldsState(pageType, propertyType, activityType, entity);
            return fieldStates.MapToResponse();
        }
    }
}
