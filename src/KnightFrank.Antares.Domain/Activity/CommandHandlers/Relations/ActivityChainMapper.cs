namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    public class ActivityChainMapper : IActivityReferenceMapper<ChainTransaction>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<ChainTransaction> chainTransactionRepository;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityControlsConfiguration;
        private readonly IEnumParser enumParser;

        public ActivityChainMapper(
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<ChainTransaction> chainTransactionRepository,
            IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityControlsConfiguration,
            IEnumParser enumParser)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.chainTransactionRepository = chainTransactionRepository;
            this.activityControlsConfiguration = activityControlsConfiguration;
            this.enumParser = enumParser;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            ActivityType activityType = this.enumParser.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(activity.ActivityTypeId);
            PropertyType propertyType = this.enumParser.Parse<Dal.Model.Property.PropertyType, PropertyType>(activity.Property.PropertyTypeId);
            IList<InnerFieldState> configuration = this.activityControlsConfiguration.GetInnerFieldsState(PageType.Update,
                new Tuple<PropertyType, ActivityType>(propertyType, activityType), message);

            if (configuration.Any(x => x.ControlCode == ControlCode.Offer_UpwardChain && x.Readonly))
            {
                return;
            }

            this.Validate(message, activity);

            activity.ChainTransactions
                    .Where(x => message.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                    .ToList()
                    .ForEach(x => this.chainTransactionRepository.Delete(x));

            message.ChainTransactions
                   .Where(x => activity.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                   .ToList()
                   .ForEach(x => activity.ChainTransactions.Add(x));

            IEnumerable<ChainTransaction> existingChains = message.ChainTransactions
                                        .Where(x => activity.ChainTransactions.Select(y => y.Id).Contains(x.Id));

            foreach (ChainTransaction existingChain in existingChains)
            {
                ChainTransaction chainToUpdate = activity.ChainTransactions.Single(x => x.Id == existingChain.Id);
                Mapper.Map(existingChain, chainToUpdate);
            }
        }

        private void Validate(ActivityCommandBase message, Activity activity)
        {
            if (message.ChainTransactions.Count(x => x.IsEnd) > 1)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_ChainTransactions_IsEndCountMoreThenOne);
            }

            foreach (ChainTransaction chainTransaction in message.ChainTransactions)
            {
                this.enumTypeItemValidator.ItemExists(EnumType.ChainMortgageSurveyStatus, chainTransaction.SurveyId);
                this.enumTypeItemValidator.ItemExists(EnumType.ChainMortgageStatus, chainTransaction.MortgageId);
                this.enumTypeItemValidator.ItemExists(EnumType.ChainSearchStatus, chainTransaction.SearchesId);
                this.enumTypeItemValidator.ItemExists(EnumType.ChainContractAgreedStatus, chainTransaction.ContractAgreedId);
                this.enumTypeItemValidator.ItemExists(EnumType.ChainEnquiries, chainTransaction.EnquiriesId);
                this.entityValidator.EntityExists<Requirement>(chainTransaction.RequirementId);
                this.entityValidator.EntityExists<Activity>(chainTransaction.ActivityId);
                this.entityValidator.EntityExists<Property>(chainTransaction.PropertyId);
                this.entityValidator.EntityExists<Company>(chainTransaction.SolicitorCompanyId);
                this.entityValidator.EntityExists<Contact>(chainTransaction.SolicitorContactId);
                this.entityValidator.EntityExists<ChainTransaction>(chainTransaction.ParentId);
                if (chainTransaction.AgentUserId.HasValue)
                {
                    this.entityValidator.EntityExists<User>(chainTransaction.AgentUserId);
                    if (chainTransaction.AgentContactId.HasValue || chainTransaction.AgentCompanyId.HasValue)
                    {
                        throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAgentContact_InvalidValue);
                    }
                }
                else
                {
                    this.entityValidator.EntityExists<Company>(chainTransaction.AgentCompanyId);
                    this.entityValidator.EntityExists<Contact>(chainTransaction.AgentContactId);
                    if (chainTransaction.AgentUserId != null)
                    {
                        throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAgentUser_InvalidValue);
                    }
                }

                if (chainTransaction.Vendor?.Length > 128)
                {
                    throw new BusinessValidationException(ErrorMessage.ActivityVendor_ValueToLong);
                }

                ChainTransaction nextChainTransaction =
                    message.ChainTransactions.FirstOrDefault(x => x.ParentId == chainTransaction.Id);
                if (chainTransaction.IsEnd && nextChainTransaction != null)
                {
                    throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionEnd_InvalidValue);
                }
            }
            this.ValidateAddRemove(message, activity);
            this.ValidateAddedChains(message, activity);
            this.ValidateRemovedChains(message, activity);
        }

        private void ValidateAddRemove(ActivityCommandBase message, Activity activity)
        {
            List<ChainTransaction> chainsToAdd = message.ChainTransactions
                                .Where(x => activity.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                .ToList();

            List<ChainTransaction> chainsToRemove = activity.ChainTransactions
                                      .Where(x => message.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                      .ToList();

            if (chainsToAdd.Count > 0 && chainsToRemove.Count > 0)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAddRemove_AtSameTime);
            }

        }

        private void ValidateAddedChains(ActivityCommandBase message, Activity activity)
        {
            List<ChainTransaction> chainsToAdd = message.ChainTransactions
                                .Where(x => activity.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                .ToList();

            if (chainsToAdd.Count == 0)
                return;

            if(chainsToAdd.Count > 1)
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAdd_MoreThanOne);

            if (activity.ChainTransactions.Count == 0)
                return;

            ChainTransaction chainToAdd = chainsToAdd.Single();
            ChainTransaction lastChain = this.FindLastChain(activity.ChainTransactions);

            if (chainToAdd.ParentId != lastChain.Id)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAdd_InvalidParentId);
            }
        }

        private void ValidateRemovedChains(ActivityCommandBase message, Activity activity)
        {
            List<ChainTransaction> chainsToRemove = activity.ChainTransactions
                                      .Where(x => message.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                      .ToList();

            if (chainsToRemove.Count == 0)
                return;

            if(chainsToRemove.Count > 1)
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionRemove_MoreThanOne);

            ChainTransaction chainToAdd = chainsToRemove.Single();
            ChainTransaction lastChain = this.FindLastChain(activity.ChainTransactions);

            if (chainToAdd.Id != lastChain.Id)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionRemove_InvalidId);
            }
        }

        private ChainTransaction FindLastChain(IEnumerable<ChainTransaction> chains)
        {
            IEnumerable<ChainTransaction> chainTransactions = chains as IList<ChainTransaction> ?? chains.ToList();
            ChainTransaction firstChain = chainTransactions.First(c => c.ParentId == null);

            ChainTransaction lastChain = firstChain;
            ChainTransaction nextChain;
            do
            {
                nextChain = chainTransactions.SingleOrDefault(c => c.ParentId == lastChain.Id);
                if (nextChain != null)
                    lastChain = nextChain;
            }
            while (nextChain != null);

            return lastChain;
        }
    }
}
