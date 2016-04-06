using System.Web.Http;

namespace KnightFrank.Antares.Api.Controllers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Domain.Company.Command;

    using MediatR;

    /// <summary>
    ///     Companies controller
    /// </summary>
    [Route("api/companies")]
    public class CompaniesController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Companies controller constructor
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public CompaniesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Creates the company.
        /// </summary>
        /// <param name="command">The command.</param>
        [HttpPost]
        [Route("")]
        public Company CreateCompany([FromBody] CreateCompanyCommand command)
        {
            Guid companyId = this.mediator.Send(command);

            return this.GetCompany(companyId);
        }

        /// <summary>
        ///     Gets the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Company entity</returns>
        [HttpGet]
        [Route("{id}")]
        public Company GetCompany(Guid id)
        {
            return new Company { Id = id };
        }
    }
}
