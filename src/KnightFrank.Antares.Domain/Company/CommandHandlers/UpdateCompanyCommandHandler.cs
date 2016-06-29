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
        private readonly IGenericRepository<CompanyContact> companyContactRepository;

        public UpdateCompanyCommandHandler(IGenericRepository<Company> companyRepository,
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<CompanyContact> companyContactRepository)
        {
            this.companyRepository = companyRepository;
            this.contactRepository = contactRepository;
            this.companyContactRepository = companyContactRepository;
        }

        public Guid Handle(UpdateCompanyCommand message)
        {
            Company company = this.companyRepository.GetWithInclude(x => x.Id == message.Id).SingleOrDefault();

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

            this.RemoveExistingRelationships(company);
            this.AddNewRelationships(company, companyContacts);

            this.companyRepository.Save();

            return company.Id;
        }

        private void AddNewRelationships(Company company, List<Contact> companyContacts)
        {
            foreach (Contact companyContact in companyContacts)
            {
                company.CompaniesContacts.Add(new CompanyContact
                {
                    Contact = companyContact,
                    ContactId = companyContact.Id,
                    Company = company,
                    CompanyId = company.Id
                });
            }
        }

        private void RemoveExistingRelationships(Company company)
        {
            List<CompanyContact> companyContactsToRemove = company.CompaniesContacts.ToList();
            for (int i = 0; i < companyContactsToRemove.Count; i++)
            {
                this.companyContactRepository.Delete(companyContactsToRemove[i]);
            }

            this.companyContactRepository.Save();
        }
    }
}