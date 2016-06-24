namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreateActivityAttachmentCommandHandler :
        CreateEntityAttachmentCommandHandler<Activity, CreateActivityAttachmentCommand>
    {
         public CreateActivityAttachmentCommandHandler(IEntityValidator entityValidator, IEnumTypeItemValidator enumTypeItemValidator, IGenericRepository<Activity> activityRepository)
            : base(entityValidator, enumTypeItemValidator, activityRepository, EnumType.ActivityDocumentType)
        {
        }

        protected override void AddAttachmentToEntity(Activity entity, Attachment attachment)
        {
            entity.Attachments.Add(attachment);
        }
    }
}