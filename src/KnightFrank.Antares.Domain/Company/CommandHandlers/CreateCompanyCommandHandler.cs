namespace KnightFrank.Antares.Domain.Company.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.Internal;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Company.Commands;

    using MediatR;

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IGenericRepository<CompanyContact> companyContactRepository;

        public CreateCompanyCommandHandler(IGenericRepository<Contact> contactRepository, IGenericRepository<CompanyContact> companyContactRepository)
        {
            this.contactRepository = contactRepository;
            this.companyContactRepository = companyContactRepository;
        }

        public Guid Handle(CreateCompanyCommand message)
        {
            var company = Mapper.Map<Company>(message);

            List<Contact> companyContacts = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            if (!message.ContactIds.All(x => companyContacts.Select(c => c.Id).Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Company_Contacts_Id);
            }

            companyContacts.Each(c => this.companyContactRepository.Add(new CompanyContact { Company = company, Contact = c }));

            this.companyContactRepository.Save();

            return company.Id;
        }
    }
}