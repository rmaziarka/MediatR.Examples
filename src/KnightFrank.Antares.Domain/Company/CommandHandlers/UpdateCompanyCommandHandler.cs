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
			// Check that company exists in the database
			Company company = this.companyRepository
								  .GetWithInclude(x => x.Id == message.Id, 
												  x => x.Contacts).SingleOrDefault();

			if (company == null)
			{
				throw new BusinessValidationException(ErrorMessage.Missing_Company_Id);
			}

			Mapper.Map(message, company);

			// Check that all supplied company contacts exist in the database
			List<Contact> companyContacts = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();

			if (!message.ContactIds.All(x => companyContacts.Select(c => c.Id).Contains(x)))
			{
				throw new BusinessValidationException(ErrorMessage.Missing_Company_Contacts_Id);
			}

	        if (company.Contacts != null)
	        {
		        List<Contact> existingCompanyContacts = company.Contacts.ToList();

		        existingCompanyContacts
			        .Where(n => IsRemovedFromExistingList(n.Id, message.ContactIds))
			        .ToList()
			        .ForEach(n => this.contactRepository.Delete(n));
	        }

			message.ContactIds
					.ToList()
					.ForEach(n => company.Contacts.Add(this.contactRepository.GetById(n)));

			this.companyRepository.Save();

			return company.Id;
        }

		private static bool IsRemovedFromExistingList(
				Guid existingCompanyContactId,
				IList<Guid> commandContactIds)
		{
			return !commandContactIds.Select(x => x).Contains(existingCompanyContactId);
		}
	}
}