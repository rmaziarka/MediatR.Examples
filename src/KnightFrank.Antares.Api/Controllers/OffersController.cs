namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Offer;

    using MediatR;

    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.Offer.Queries;

    /// <summary>
    /// Offers controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/offers")]
    public class OffersController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffersController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public OffersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create offer.
        /// </summary>
        /// <returns>Offer.</returns>
        [HttpPost]
        [Route("")]
        public Offer CreateOffer(CreateOfferCommand command)
        {
            Guid offerId = this.mediator.Send(command);
            return this.GetOffer(offerId);
        }

        /// <summary>
        /// Updates the offer.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public Offer UpdateOffer(UpdateOfferCommand command)
        {
            Guid offerId = this.mediator.Send(command);
            return this.GetOffer(offerId);
        }

        /// <summary>
        /// Gets the offer by Id.
        /// </summary>
        /// <param name="id">Offer Id.</param>
        /// <returns>Offer.</returns>
        [HttpGet]
        [Route("{id}")]
        public Offer GetOffer(Guid id)
        {
            Offer offer = this.mediator.Send(new OfferQuery { Id = id });
            if (offer == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Offer not found."));
            }

            return offer;
        }
    }
}
