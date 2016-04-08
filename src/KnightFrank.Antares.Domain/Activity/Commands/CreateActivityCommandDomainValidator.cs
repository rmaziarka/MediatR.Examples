namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateActivityCommandDomainValidator : AbstractValidator<CreateActivityCommand>, IDomainValidator<CreateActivityCommand>
    {   
        public CreateActivityCommandDomainValidator(IGenericRepository<Contact> contactRepository)
        {
             this.RuleFor(p => p.ContactIds).SetValidator(new ContactValidator(contactRepository));
        }
    }
}