namespace KnightFrank.Antares.Domain.User.Queries
{
    using FluentValidation;

    public class UsersQueryValidator : AbstractValidator<UsersQuery>
    {
        public UsersQueryValidator()
        {
            this.RuleFor(q => q.PartialName).NotNull();
        }
    }
}
