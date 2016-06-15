namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.AreaBreakdown.Queries;
    using KnightFrank.Antares.Domain.Ownership.Commands;
    using KnightFrank.Antares.Domain.Ownership.Queries;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Domain.Property.QueryResults;
    using KnightFrank.Antares.Search.Common.QueryResults;
    using KnightFrank.Antares.Search.Property.Queries;
    using KnightFrank.Antares.Search.Property.QueryResults;

    using MediatR;

    /// <summary>
    ///     Controller class for properties
    /// </summary>
    [RoutePrefix("api/properties")]
    public class PropertiesController : ApiController
    {
        private readonly IMediator mediator;
        private const string LocaleCode = "en";

        /// <summary>
        ///     Properties controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public PropertiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Search property by query
        /// </summary>
        /// <param name="pageableQuery">Query</param>
        /// <returns>Pageable data with filtered properties</returns>
        [HttpGet]
        [Route("search")]
        public PageableResult<PropertyResult> SearchProperties([FromUri(Name = "")] PropertiesPageableQuery pageableQuery)
        {
            return this.mediator.Send(pageableQuery);
        }

        /// <summary>
        ///     Gets the property
        /// </summary>
        /// <param name="id">Property id</param>
        /// <returns>Property entity</returns>
        [HttpGet]
        [Route("{id}")]
        public Property GetProperty(Guid id)
        {
            Property property = this.mediator.Send(new PropertyQuery { Id = id });

            if (property == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Property not found."));
            }

            return property;
        }

        /// <summary>
        ///     Creates the property.
        /// </summary>
        /// <param name="command">Property data</param>
        /// <returns>Newly created property </returns>
        [HttpPost]
        [Route("")]
        public Property CreateProperty(CreatePropertyCommand command)
        {
            Guid propertyId = this.mediator.Send(command);
            return this.GetProperty(propertyId);
        }

        /// <summary>
        ///     Updates the property.
        /// </summary>
        /// <param name="command">Property data</param>
        /// <returns>Newly updated property</returns>
        [HttpPut]
        [Route("")]
        public Property UpdateProperty(UpdatePropertyCommand command)
        {
            Guid propertyId = this.mediator.Send(command);
            return this.GetProperty(propertyId);
        }

        /// <summary>
        ///     Creates the ownership.
        /// </summary>
        /// <param name="id">Property Id</param>
        /// <param name="command">Ownership data</param>
        /// <returns>Updated property</returns>
        [HttpPost]
        [Route("{id}/ownerships")]
        public Ownership CreateOwnership(Guid id, CreateOwnershipCommand command)
        {
            command.PropertyId = id;
            Guid ownershipId = this.mediator.Send(command);

            Ownership ownership =
                this.mediator.Send(new OwnershipByIdQuery { OwnershipId = ownershipId });

            return ownership;
        }

        /// <summary>
        /// Gets the property types.
        /// </summary>
        /// <param name="propertyTypesQuery">The property types query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public PropertyTypeQueryResult GetPropertyTypes([FromUri(Name = "")]PropertyTypeQuery propertyTypesQuery)
        {
            propertyTypesQuery = propertyTypesQuery ?? new PropertyTypeQuery();
            propertyTypesQuery.LocaleCode = LocaleCode;

            return this.mediator.Send(propertyTypesQuery);
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="propertyAttributesQuery">The property attributes query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("attributes")]
        public PropertyAttributesQueryResult GetPropertyAttributes([FromUri(Name = "")]PropertyAttributesQuery propertyAttributesQuery)
        {
            return this.mediator.Send(propertyAttributesQuery);
        }

        /// <summary>
        /// Creates the area breakdown.
        /// </summary>
        /// <param name="id">Property id.</param>
        /// <param name="command">Command data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/areabreakdown")]
        public IList<PropertyAreaBreakdown> CreateAreaBreakdown(Guid id, CreateAreaBreakdownCommand command)
        {
            command.PropertyId = id;
            IList<Guid> areaIds = this.mediator.Send(command);

            return this.mediator.Send(new AreaBreakdownQuery { PropertyId = command.PropertyId, AreaIds = areaIds });
        }

        /// <summary>
        /// Update the area breakdown name and size.
        /// </summary>
        /// <param name="id">Property id.</param>
        /// <param name="command">Command data.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/areabreakdown")]
        public PropertyAreaBreakdown UpdateAreaBreakdown(Guid id, UpdateAreaBreakdownCommand command)
        {
            command.PropertyId = id;
            Guid propertyAreaBreakdownId = this.mediator.Send(command);

            var query = new AreaBreakdownQuery { PropertyId = command.PropertyId, AreaIds = new[] { propertyAreaBreakdownId } };
            return this.mediator.Send(query).SingleOrDefault();
        }

        /// <summary>
        /// Updates the area breakdown order.
        /// </summary>
        /// <param name="id">Property id.</param>
        /// <param name="command">Command data.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/areabreakdown/order")]
        public IList<PropertyAreaBreakdown> UpdateAreaBreakdownOrder(Guid id, UpdateAreaBreakdownOrderCommand command)
        {
            command.PropertyId = id;
            this.mediator.Send(command);

            return this.mediator.Send(new AreaBreakdownQuery { PropertyId = command.PropertyId});
        } 
    }
}
