namespace KnightFrank.Antares.Domain.User.Queries
{
    using FluentValidation;

    public class UserQueryValidator : AbstractValidator<UserQuery>
    {
        public UserQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotEmpty();
        }
    }
}
