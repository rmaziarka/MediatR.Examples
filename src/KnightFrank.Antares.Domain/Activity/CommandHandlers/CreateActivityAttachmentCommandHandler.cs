namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreateActivityAttachmentCommandHandler : IRequestHandler<CreateEntityAttachmentCommand, Guid>
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

        public Guid Handle(CreateEntityAttachmentCommand message)
        {
            this.entityValidator.EntityExists<User>(message.Attachment.UserId);
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityDocumentType, message.Attachment.DocumentTypeId);

            Activity activity = this.activityRepository.GetById(message.EntityId);
            this.entityValidator.EntityExists(activity, message.EntityId);

            var attachment = AutoMapper.Mapper.Map<Attachment>(message.Attachment);

            activity.Attachments.Add(attachment);
            this.activityRepository.Save();

            return attachment.Id;
        }
    }
}