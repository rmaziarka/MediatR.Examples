namespace KnightFrank.Antares.Domain.Company.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Company.Commands;

    using MediatR;

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly IGenericRepository<Company> companyRepository;
        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IDomainValidator<CreateCompanyCommand> domainValidator;

        public CreateCompanyCommandHandler(IGenericRepository<Company> companyRepository, IGenericRepository<Contact> contactRepository, IDomainValidator<CreateCompanyCommand> domainValidator)
        {
            this.companyRepository = companyRepository;
            this.contactRepository = contactRepository;
            this.domainValidator = domainValidator;
        }

        public Guid Handle(CreateCompanyCommand message)
        {
            ValidationResult validationResult = this.domainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var company = Mapper.Map<Company>(message);

            List<Contact> companyContacts = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            company.Contacts = companyContacts;

            this.companyRepository.Add(company);
            this.companyRepository.Save();

            return company.Id;
        }
    }
}