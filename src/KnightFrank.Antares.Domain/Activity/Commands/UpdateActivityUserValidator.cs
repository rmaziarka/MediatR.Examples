namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityUserValidator : AbstractValidator<UpdateActivityUser>
    {
        public UpdateActivityUserValidator()
        {
            this.RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
