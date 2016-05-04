namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Specifications;

    using MediatR;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreateActivityAttachmentCommandHandler : IRequestHandler<CreateActivityAttachmentCommand, Guid>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<Activity> activityRepository;

        public CreateActivityAttachmentCommandHandler(
            IEntityValidator entityValidator, 
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<Activity> activityRepository)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.activityRepository = activityRepository;
        }

        public Guid Handle(CreateActivityAttachmentCommand message)
        {
            this.entityValidator.EntityExits<Activity>(message.ActivityId);
            this.enumTypeItemValidator.ItemExists(new EnumTypeExists(EnumType.ActivityDocumentType), message.Attachment.DocumentTypeId);

            Activity activity = this.activityRepository.GetById(message.ActivityId);
            this.entityValidator.EntityExits(activity, message.ActivityId);

            Attachment attachment = AutoMapper.Mapper.Map<Attachment>(message.Attachment);

            activity.Attachments.Add(attachment);
            this.activityRepository.Save();

            return attachment.Id;
        }
    }
}