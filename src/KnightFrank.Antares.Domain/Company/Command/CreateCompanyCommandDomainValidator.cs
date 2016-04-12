namespace KnightFrank.Antares.Domain.Company.Command
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateCompanyCommandDomainValidator : AbstractValidator<CreateCompanyCommand>, IDomainValidator<CreateCompanyCommand>
    {   
        public CreateCompanyCommandDomainValidator(IGenericRepository<Contact> contactRepository)
        {   
            this.RuleFor(x => x.ContactIds).SetValidator(new ContactValidator(contactRepository));
        }
    }
}