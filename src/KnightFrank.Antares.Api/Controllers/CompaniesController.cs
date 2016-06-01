using System.Web.Http;

namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Domain.Company.Commands;
    using KnightFrank.Antares.Domain.Contact.Queries;

    using MediatR;

    /// <summary>
    ///     Companies controller
    /// </summary>
    [RoutePrefix("api/companies")]
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
		/// Updates a company
		/// </summary>
		/// <param name="command">The command</param>
		/// <returns></returns>
	    [HttpPut]
	    [Route("")]
	    public Company UpdateCompany(UpdateCompanyCommand command)
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
            var query = new CompanyQuery() { Id = id };

		    Company company = this.mediator.Send(query);

            if (company == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Company not found."));
            }

            return company;
          
        }
    }
}
