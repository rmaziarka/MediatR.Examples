namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateViewingCommandValidator : AbstractValidator<CreateViewingCommand>
    {
        public CreateViewingCommandValidator()
        {
        }
    }
}