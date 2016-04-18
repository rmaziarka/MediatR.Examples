namespace KnightFrank.Antares.Domain.RequirementNote.CommandHandlers
{
    using System;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.RequirementNote.Commands;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class CreateRequirementNoteCommandHandler : IRequestHandler<CreateRequirementNoteCommand, Guid>
    {
        private readonly IGenericRepository<RequirementNote> requirementNoteRepository;
        private readonly IGenericRepository<User> userRepository;
        private readonly IDomainValidator<CreateRequirementNoteCommand> domainValidator;

        public CreateRequirementNoteCommandHandler(IGenericRepository<RequirementNote> requirementNoteRepository, IGenericRepository<User> userRepository, IDomainValidator<CreateRequirementNoteCommand> domainValidator)
        {
            this.requirementNoteRepository = requirementNoteRepository;
            this.userRepository = userRepository;
            this.domainValidator = domainValidator;
        }

        public Guid Handle(CreateRequirementNoteCommand message)
        {
            ValidationResult validationResult = this.domainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var requirementNote = AutoMapper.Mapper.Map<RequirementNote>(message);

            //TODO: set proper user
            requirementNote.UserId = this.userRepository.FindBy(u => true).First().Id;

            this.requirementNoteRepository.Add(requirementNote);
            this.requirementNoteRepository.Save();

            return requirementNote.Id;
        }
    }
}
