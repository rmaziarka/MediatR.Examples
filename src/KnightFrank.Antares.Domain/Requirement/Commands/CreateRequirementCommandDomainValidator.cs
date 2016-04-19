namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateRequirementCommandDomainValidator : AbstractValidator<CreateRequirementCommand>, IDomainValidator<CreateRequirementCommand>
    {
        public CreateRequirementCommandDomainValidator(IGenericRepository<Contact> contactRepository)
        {
            this.RuleFor(x => x.ContactIds).SetValidator(new ContactValidator(contactRepository));
        }
    }
}
