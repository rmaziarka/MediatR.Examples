namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreatePropertyAttachmentCommandHandler : IRequestHandler<CreateEntityAttachmentCommand, Guid>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<Property> propertyRepository;

        public CreatePropertyAttachmentCommandHandler(
            IEntityValidator entityValidator, 
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<Property> propertyRepository)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(CreateEntityAttachmentCommand message)
        {
            this.entityValidator.EntityExists<User>(message.Attachment.UserId);
            this.enumTypeItemValidator.ItemExists(EnumType.PropertyDocumentType, message.Attachment.DocumentTypeId);

            Property property = this.propertyRepository.GetById(message.EntityId);
            this.entityValidator.EntityExists(property, message.EntityId);

            var attachment = AutoMapper.Mapper.Map<Attachment>(message.Attachment);

            property.Attachments.Add(attachment);
            this.propertyRepository.Save();

            return attachment.Id;
        }
    }
}