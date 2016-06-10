namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityDepartmentValidator : AbstractValidator<UpdateActivityDepartment>
    {
        public UpdateActivityDepartmentValidator()
        {
            this.RuleFor(x => x.DepartmentId).NotEmpty();
            this.RuleFor(x => x.DepartmentTypeId).NotEmpty();
        }
    }
}
