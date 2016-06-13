namespace KnightFrank.Antares.Domain.User.CommandHandlers
{
    using System;

    using Dal.Repository;
    using Common.BusinessValidators;
    using Common.Enums;
    using Commands;

    using KnightFrank.Antares.Dal.Model.User;

    using MediatR;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IGenericRepository<User> userRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public UpdateUserCommandHandler(
            IGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator)
        {
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
        }

        public Guid Handle(UpdateUserCommand message)
        {
            User user = this.userRepository.GetById(message.Id);
            this.entityValidator.EntityExists(user, message.Id);

            if (message.SalutationFormatId != null)
            {
                this.enumTypeItemValidator.ItemExists(EnumType.SalutationFormat,(Guid) message.SalutationFormatId);
            }

            AutoMapper.Mapper.Map(message, user);

            this.userRepository.Save();

            return user.Id;
        }
    }
}
