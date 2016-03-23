namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;
    using System;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    ///     Controller class for address form
    /// </summary>
    [RoutePrefix("api/addressform")]
    public class AddressFormController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     AddressForm controller constructor
        /// </summary>
        /// <param name="mediator">Command /query mediator</param>
        public AddressFormController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Get AddressForm for defined entity type and country code
        /// </summary>
        /// <param name="addressFormQuery">Query by entity type and country code</param>
        /// <returns></returns>
        [HttpGet]
        public AddressFormQueryResult GetAddressFormQueryResult([FromUri(Name = "")] AddressFormQuery addressFormQuery)
        {
            return this.mediator.Send(addressFormQuery);
        }

        /// <summary>
        ///    Get all countries that have defined address form for given entity type
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("countries/")]
        public IList<CountryLocalisedResult> GetCountries([FromUri(Name = "")] GetCountriesForAddressFormsQuery query)
        {
            return this.mediator.Send(query);
        }

        /// <summary>
        /// Gets the address form query result.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Address form query result </returns>
        [HttpGet]
        [Route("{id}")]
        public AddressFormQueryResult GetAddressFormQueryResult(Guid id)
        {
            var result = this.mediator.Send(new AddressFormByIdQuery(id));
            if (result == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Address form not found."));
            }

            return result;
        }
    }
}
