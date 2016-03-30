using System.Web.Http;

namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;

    /// <summary>
    /// Controller class for resources.
    /// </summary>
    [RoutePrefix("api/resources")]
    public class ResourcesController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public ResourcesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///    Get all countries that have defined address form for given entity type
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("countries/addressform")]
        public IList<CountryLocalisedResult> GetCountries([FromUri(Name = "")] GetCountriesForAddressFormsQuery query)
        {
            return this.mediator.Send(query);
        }
    }
}
