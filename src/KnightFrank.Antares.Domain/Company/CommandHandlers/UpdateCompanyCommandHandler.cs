namespace KnightFrank.Antares.Domain.Company.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Company.Commands;

    using MediatR;

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Guid>
    {
        private readonly IGenericRepository<Company> companyRepository;
        private readonly IGenericRepository<Contact> contactRepository;

        public UpdateCompanyCommandHandler(IGenericRepository<Company> companyRepository,
            IGenericRepository<Contact> contactRepository)
        {
            this.companyRepository = companyRepository;
            this.contactRepository = contactRepository;
        }

        public Guid Handle(UpdateCompanyCommand message)
        {
			Company company = this.companyRepository
								  .GetWithInclude(x => x.Id == message.Id,
												  x => x.Contacts).SingleOrDefault();

			if (company == null)
			{
				throw new BusinessValidationException(ErrorMessage.Missing_Company_Id);
			}

			// TODO: validate unique collection

			Mapper.Map(message, company);

			List<Guid> commandContactIds = message.Contacts.Select(c => c.Id).ToList();

			List<Contact> companyContacts = this.contactRepository.FindBy(x => commandContactIds.Contains(x.Id)).ToList();

			if (!message.Contacts.Select(c => c.Id).All(x => companyContacts
								 .Select(c => c.Id).Contains(x)))
			{
				throw new BusinessValidationException(ErrorMessage.Missing_Company_Contacts_Id);
			}

			company.Contacts = companyContacts;

			this.companyRepository.Save();

			return company.Id;
        }
	}
}