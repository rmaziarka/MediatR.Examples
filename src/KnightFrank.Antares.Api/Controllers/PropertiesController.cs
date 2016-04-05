namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Commands;
    using KnightFrank.Antares.Domain.Ownership.Queries;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;

    /// <summary>
    ///     Controller class for properties
    /// </summary>
    [RoutePrefix("api/properties")]
    public class PropertiesController : ApiController
    {
        private readonly IMediator mediator;
        private const string LocaleCode = "en";
        private readonly IReadGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository;

        /// <summary>
        ///     Properties controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        /// <param name="propertyTypeDefinitionRepository"></param>
        public PropertiesController(IMediator mediator, IReadGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository)
        {
            this.mediator = mediator;
            this.propertyTypeDefinitionRepository = propertyTypeDefinitionRepository;
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
            // TODO Quick fix - to be removed after propertyTypeId is sent from GUI
            if (command.PropertyTypeId == Guid.Empty)
            {
                command.PropertyTypeId = this.propertyTypeDefinitionRepository.Get().First(x => x.Country.IsoCode == "GB").PropertyTypeId;
            }

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
                this.mediator.Send(new OwnershipByIdQuery() { OwnershipId = ownershipId });

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
    }
}
