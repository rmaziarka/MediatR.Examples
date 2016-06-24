namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    public abstract class CreateEntityAttachmentCommandHandler<TEntity, TCreateEntityAttachmentCommand> : IRequestHandler<TCreateEntityAttachmentCommand, Guid> 
        where TEntity : BaseEntity 
        where TCreateEntityAttachmentCommand : CreateEntityAttachmentCommand
    {
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<TEntity> entityRepository;
        private readonly EnumType enumType;

        protected CreateEntityAttachmentCommandHandler(
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<TEntity> entityRepository,
            EnumType enumType)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.entityRepository = entityRepository;
            this.enumType = enumType;
        }

        public Guid Handle(TCreateEntityAttachmentCommand message)
        {
            this.entityValidator.EntityExists<User>(message.Attachment.UserId);
            this.enumTypeItemValidator.ItemExists(this.enumType, message.Attachment.DocumentTypeId);

            TEntity entity = this.entityRepository.GetById(message.EntityId);
            this.entityValidator.EntityExists(entity, message.EntityId);

            var attachment = AutoMapper.Mapper.Map<Attachment>(message.Attachment);

            this.AddAttachmentToEntity(entity, attachment);
            this.entityRepository.Save();

            return attachment.Id;
        }

        protected abstract void AddAttachmentToEntity(TEntity entity, Attachment attachment);
    }
}
