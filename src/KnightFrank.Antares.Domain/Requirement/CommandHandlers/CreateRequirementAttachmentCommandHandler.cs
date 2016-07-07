namespace KnightFrank.Antares.Domain.Requirement.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    public class CreateRequirementAttachmentCommandHandler : CreateEntityAttachmentCommandHandler<Requirement, CreateRequirementAttachmentCommand>
    {
        public CreateRequirementAttachmentCommandHandler(IEntityValidator entityValidator, IEnumTypeItemValidator enumTypeItemValidator, IGenericRepository<Requirement> requirementRepository)
           : base(entityValidator, enumTypeItemValidator, requirementRepository, EnumType.RequirementDocumentType)
        {
        }

        protected override void AddAttachmentToEntity(Requirement entity, Attachment attachment)
        {
            entity.Attachments.Add(attachment);
        }
    }
}
