﻿namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Dal.Model;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Domain.Property.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for properties
    /// </summary>
    public class PropertyController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Properties controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public PropertyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Gets the property
        /// </summary>
        /// <param name="id">Property id</param>
        /// <returns>Property entity</returns>
        [HttpGet]
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
        /// <param name="command">Command payload.</param>
        /// <returns>New </returns>
        [HttpPost]
        public Property CreateProperty(CreatePropertyCommand command)
        {
            return this.mediator.Send(command);
        }

        /// <summary>
        ///     Updates the property.
        /// </summary>
        /// <param name="command">Command payload.</param>
        /// <returns>Newly updated property id</returns>
        [HttpPut]
        public Guid UpdateProperty(UpdatePropertyCommand command)
        {
            return this.mediator.Send(command);
        }
    }
}