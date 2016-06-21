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

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly IGenericRepository<Company> companyRepository;
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateCompanyCommandHandler(IGenericRepository<Company> companyRepository,
            IGenericRepository<Contact> contactRepository)
        {
            this.companyRepository = companyRepository;
            this.contactRepository = contactRepository;
        }

        public Guid Handle(CreateCompanyCommand message)
        {
            var company = Mapper.Map<Company>(message);

            List<Contact> companyContacts = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            if (!message.ContactIds.All(x => companyContacts.Select(c => c.Id).Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Company_Contacts_Id);
            }

            company.Contacts = companyContacts;

            this.companyRepository.Add(company);
            this.companyRepository.Save();

            return company.Id;
        }
    }
}