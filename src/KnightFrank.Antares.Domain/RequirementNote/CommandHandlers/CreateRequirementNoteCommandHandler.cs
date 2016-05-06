namespace KnightFrank.Antares.Domain.RequirementNote.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Domain.RequirementNote.Commands;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class CreateRequirementNoteCommandHandler : IRequestHandler<CreateRequirementNoteCommand, Guid>
    {
        private readonly IGenericRepository<RequirementNote> requirementNoteRepository;
        private readonly IGenericRepository<User> userRepository;
        private readonly IEntityValidator entityValidator;

        public CreateRequirementNoteCommandHandler(IGenericRepository<RequirementNote> requirementNoteRepository, IGenericRepository<User> userRepository, IEntityValidator entityValidator)
        {
            this.requirementNoteRepository = requirementNoteRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
        }

        public Guid Handle(CreateRequirementNoteCommand message)
        {
            this.entityValidator.EntityExists<Requirement>(message.RequirementId);

            var requirementNote = AutoMapper.Mapper.Map<RequirementNote>(message);

            //TODO: set proper user
            requirementNote.UserId = this.userRepository.FindBy(u => true).First().Id;

            this.requirementNoteRepository.Add(requirementNote);
            this.requirementNoteRepository.Save();

            return requirementNote.Id;
        }
    }
}
