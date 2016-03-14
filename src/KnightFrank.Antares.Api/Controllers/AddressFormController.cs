namespace KnightFrank.Antares.Api.Controllers
{
    using System.Web.Http;

    using KnightFrank.Antares.Domain.AddressForm;

    using MediatR;

    /// <summary>
    ///     Controller class for address form
    /// </summary>
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
    }
}
