namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove;

    /// <summary>
    /// Metadata controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/metadata")]
    public class MetadataController : ApiController
    {
        private readonly IControlsConfiguration<PropertyType, ActivityType> activityConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataController"/> class.
        /// </summary>
        /// <param name="activityConfiguration">The activity configuration.</param>
        public MetadataController(IControlsConfiguration<PropertyType, ActivityType> activityConfiguration)
        {
            this.activityConfiguration = activityConfiguration;
        }

        /// <summary>
        /// Gets the activity configuration.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="activityType">Type of the activity.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("attributes/activity")]
        public dynamic GetActivityConfiguration(PageType pageType, PropertyType propertyType, ActivityType activityType, CreateCommand entity)
        {
            IList<InnerFieldState> fieldStates = this.activityConfiguration.GetInnerFieldsState(pageType, propertyType, activityType, entity);
            return fieldStates.MapToResponse();
        }
    }
}
