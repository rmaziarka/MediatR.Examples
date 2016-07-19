namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Api;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Domain.Tenancy.Queries;
    using KnightFrank.Antares.Domain.Tenancy.QueryResults;

    using MediatR;

    /// <summary>
    ///     Controller class for tenancies
    /// </summary>
    [RoutePrefix("api/tenancies")]
    public class TenanciesController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Tenancies controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public TenanciesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Creates the tenancy
        /// </summary>
        /// <param name="command">Tenancy data to create</param>
        [HttpPost]
        [Route("")]
        public Tenancy CreateTenancy([FromBody] CreateTenancyCommand command)
        {
            Guid activityId = this.mediator.Send(command);

            return this.GetTenancy(activityId);
        }

        /// <summary>
        ///     Gets the tenancy
        /// </summary>
        /// <param name="id">Tenancy id</param>
        /// <returns>Tenancy entity</returns>
        [HttpGet]
        [Route("{id}")]
        public Tenancy GetTenancy(Guid id)
        {
            Tenancy tenancy = this.mediator.Send(new TenancyQuery { Id = id });

            if (tenancy == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tenancy not found."));
            }

            return tenancy;
        }

        /// <summary>
        ///     Updates the tenancy
        /// </summary>
        /// <param name="command">Tenancy data to update</param>
        /// <returns>Tenancy entity</returns>
        [HttpPut]
        [Route("")]
        public Tenancy UpdateTenancy(UpdateTenancyCommand command)
        {
            Guid tenancyId = this.mediator.Send(command);

            return this.GetTenancy(tenancyId);
        }


        /// <summary>
        /// Gets the tenancy types.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public IList<TenancyTypeQueryResult> GetTenancyTypes([FromUri] TenancyTypeQuery query)
        {
            query = query ?? new TenancyTypeQuery();
            return this.mediator.Send(query);
        }
    }
}
