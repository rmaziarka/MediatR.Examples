namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityUserValidator : AbstractValidator<UpdateActivityUser>
    {
        public UpdateActivityUserValidator(bool callDateRequired)
        {
            this.RuleFor(x => x.UserId).NotEmpty();
            if (callDateRequired)
            {
                this.RuleFor(x => x.CallDate).NotEmpty();
            }
        }
    }
}
