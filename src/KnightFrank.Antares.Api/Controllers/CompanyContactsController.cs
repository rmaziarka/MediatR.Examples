using System.Web.Http;

namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Domain.Company.Queries;

    using MediatR;

    /// <summary>
    ///     Companies controller
    /// </summary>
    [RoutePrefix("api/companycontacts")]
    public class CompanyContactsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Company contacts controller constructor
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public CompanyContactsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        /// <summary>
        ///     Get company contacts list
        /// </summary>
        /// <returns>Company contact entity collection</returns>
        [HttpGet]
        public IEnumerable<CompanyContact> GetCompanyContacts()
        {
            return this.mediator.Send(new CompanyContactsQuery());
        }
    }
}
