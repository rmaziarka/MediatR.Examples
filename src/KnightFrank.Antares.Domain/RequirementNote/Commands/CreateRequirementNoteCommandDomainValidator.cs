namespace KnightFrank.Antares.Domain.RequirementNote.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreateRequirementNoteCommandDomainValidator : AbstractValidator<CreateRequirementNoteCommand>, IDomainValidator<CreateRequirementNoteCommand>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;

        public CreateRequirementNoteCommandDomainValidator(IGenericRepository<Requirement> requirementRepository)
        {
            this.requirementRepository = requirementRepository;

            this.Custom(this.RequirementExists);
        }

        private ValidationFailure RequirementExists(CreateRequirementNoteCommand command)
        {
            Requirement requirement = this.requirementRepository.GetById(command.RequirementId);

            if (requirement == null)
            {
                return new ValidationFailure(nameof(command.RequirementId), "Invalid requirement has been provided.");
            }

            return null;
        }
    }
}
