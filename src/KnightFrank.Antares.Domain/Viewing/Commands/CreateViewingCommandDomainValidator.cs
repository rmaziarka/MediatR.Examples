namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateViewingCommandDomainValidator : AbstractValidator<CreateViewingCommand>, IDomainValidator<CreateViewingCommand>
    {
        public CreateViewingCommandDomainValidator(IGenericRepository<Contact> contactRepository)
        {
            this.RuleFor(x => x.AttendeesIds).SetValidator(new ContactValidator(contactRepository));
        }
    }
}
