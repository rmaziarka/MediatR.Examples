namespace KnightFrank.Antares.Domain.Company.Commands
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;

    using MediatR;

    public class UpdateCompanyCommand : IRequest<Guid>
    {
        public UpdateCompanyCommand()
        {
            this.Contacts = new List<Contact>();
        }

		public Guid Id { get; set; }

		public string Name { get; set; }

        public string WebsiteUrl { get; set; }

        public string Description { get; set; }

        public bool Valid { get; set; }

        public string ClientCarePageUrl { get; set; }

        public Guid? CompanyTypeId { get; set; }

        public Guid? CompanyCategoryId { get; set; }

        public Guid? ClientCareStatusId { get; set; }
        
        public IList<Contact> Contacts { get; set; }

        public Guid? RelationshipManagerId { get; set; }
    }
}