namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;
    using KnightFrank.Antares.Domain.Country.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for address form
    /// </summary>
    [RoutePrefix("api/addressforms")]
    public class AddressFormsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     AddressForm controller constructor
        /// </summary>
        /// <param name="mediator">Command /query mediator</param>
        public AddressFormsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Get AddressForm for defined entity type and country code
        /// </summary>
        /// <param name="addressFormQuery">Query by entity type and country code</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public AddressFormQueryResult GetAddressFormQueryResult([FromUri(Name = "")] AddressFormQuery addressFormQuery)
        {
            return this.mediator.Send(addressFormQuery);
        }

        /// <summary>
        ///     Gets the address form query result.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Address form query result </returns>
        [HttpGet]
        [Route("{id}")]
        public AddressFormQueryResult GetAddressFormQueryResult(Guid id)
        {
            AddressFormQueryResult result = this.mediator.Send(new AddressFormByIdQuery(id));
            if (result == null)
            {
                throw new HttpResponseException(
                    this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Address form not found."));
            }

            return result;
        }

        /// <summary>
        /// Gets the address form for all countries.
        /// </summary>
        /// <param name="entityType">Entity type parameter.</param>
        /// <returns>List of address forms.</returns>
        [HttpGet]
        [Route("list")]
        public Dictionary<Guid, AddressFormQueryResult> GetAddressFormForAllCountries(string entityType)
        {
            var queryResult = new Dictionary<Guid, AddressFormQueryResult>();
            List<Country> countries = this.mediator.Send(new CountriesQuery()).ToList();
            
            countries.ForEach(
                x =>
                    {
                        AddressFormQueryResult addressFormQueryResult = this.GetAddressFormQueryResult(new AddressFormQuery { CountryCode = x.IsoCode, EntityType = entityType });

                        if (addressFormQueryResult != null)
                        {
                            queryResult.Add(x.Id, addressFormQueryResult);
                        }
                    });

            return queryResult;
        }
    }
}
