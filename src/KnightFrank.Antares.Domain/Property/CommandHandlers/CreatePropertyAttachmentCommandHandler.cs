namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Property.Commands;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreatePropertyAttachmentCommandHandler :
        CreateEntityAttachmentCommandHandler<Property, CreatePropertyAttachmentCommand>
    {
        public CreatePropertyAttachmentCommandHandler(IEntityValidator entityValidator, IEnumTypeItemValidator enumTypeItemValidator, IGenericRepository<Property> propertyRepository)
            : base(entityValidator, enumTypeItemValidator, propertyRepository, EnumType.PropertyDocumentType)
        {
        }

        protected override void AddAttachmentToEntity(Property entity, Attachment attachment)
        {
            entity.Attachments.Add(attachment);
        }
    }
}
