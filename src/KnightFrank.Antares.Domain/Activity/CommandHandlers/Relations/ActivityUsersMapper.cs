namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class ActivityUsersMapper : IActivityReferenceMapper<ActivityUser>
    {
        private readonly IGenericRepository<User> userRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly ICollectionValidator collectionValidator;

        public ActivityUsersMapper(IGenericRepository<User> userRepository, IGenericRepository<EnumTypeItem> enumTypeItemRepository, ICollectionValidator collectionValidator)
        {
            this.userRepository = userRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.collectionValidator = collectionValidator;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            if (message.LeadNegotiator == null)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_LeadNegotiator_Is_Required);
            }

            var negotiators = message.SecondaryNegotiators
                .Select(x => new { UserId = x.UserId, CallDate = x.CallDate, IsLead = false })
                .Union(new[] { new { UserId = message.LeadNegotiator.UserId, CallDate = message.LeadNegotiator.CallDate, IsLead = true } })
                .ToList();

            List<Guid> negotiatorsIds = negotiators.Select(x => x.UserId).ToList();

            this.collectionValidator.CollectionIsUnique(negotiatorsIds, ErrorMessage.Activity_Negotiators_Not_Unique);

            this.collectionValidator.CollectionContainsAll(
                this.userRepository.FindBy(x => negotiatorsIds.Contains(x.Id)).Select(x => x.Id).ToList(),
                negotiatorsIds,
                ErrorMessage.Missing_Activity_Negotiators_Id);

            EnumTypeItem leadNegotiatorUserType = this.GetLeadNegotiatorUserType();
            EnumTypeItem secondaryNegotiatorUserType = this.GetSecondaryNegotiatorUserType();

            foreach (ActivityUser userToDelete in activity.ActivityUsers.Where(x => negotiatorsIds.Contains(x.UserId) == false).ToList())
            {
                activity.ActivityUsers.Remove(userToDelete);
            }

            foreach (var negotiator in negotiators)
            {
                ActivityUser existingUser = activity.ActivityUsers.SingleOrDefault(u => u.UserId == negotiator.UserId);
                if (existingUser == null)
                {
                    if (negotiator.CallDate.HasValue && negotiator.CallDate.Value.Date < DateTime.UtcNow.Date)
                    {
                        throw new BusinessValidationException(ErrorMessage.Activity_Negotiator_CallDate_InPast);
                    }

                    existingUser = new ActivityUser { UserId = negotiator.UserId };
                    activity.ActivityUsers.Add(existingUser);
                }
                else if (negotiator.CallDate.HasValue && negotiator.CallDate.Value.Date < DateTime.UtcNow.Date &&
                    (!existingUser.CallDate.HasValue || existingUser.CallDate.Value != negotiator.CallDate.Value))
                {
                    throw new BusinessValidationException(ErrorMessage.Activity_Negotiator_CallDate_InPast);
                }

                existingUser.CallDate = negotiator.CallDate;
                existingUser.UserType = negotiator.IsLead ? leadNegotiatorUserType : secondaryNegotiatorUserType;
            }
        }

        private EnumTypeItem GetLeadNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityUserType.LeadNegotiator.ToString()).Single();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityUserType.SecondaryNegotiator.ToString()).Single();
        }

    }
}