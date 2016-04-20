namespace KnightFrank.Antares.API.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Domain.Characteristic.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    [RoutePrefix("api/characteristicGroups")]
    public class CharacteristicGroupsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Characteristic groups controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public CharacteristicGroupsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Get characteristic groups
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
        public IEnumerable<CharacteristicGroupUsage> GetCharacteristicGroups(CharacteristicGroupsQuery query)
        {
            return this.mediator.Send(query);
        }

    }
}
