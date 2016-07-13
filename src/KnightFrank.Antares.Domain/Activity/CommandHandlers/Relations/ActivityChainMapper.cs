namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
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
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class ActivityChainMapper : IActivityReferenceMapper<ChainTransaction>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<ChainTransaction> chainTransactionRepository;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public ActivityChainMapper(
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<ChainTransaction> chainTransactionRepository)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.chainTransactionRepository = chainTransactionRepository;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
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

            this.ValidateAddedChains(message, activity);
            this.ValidateRemovedChains(message, activity);
        }

        private void ValidateAddedChains(ActivityCommandBase message, Activity activity)
        {
            var chainsToAdd = message.ChainTransactions
                                .Where(x => activity.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                .ToList();

            if (chainsToAdd.Count == 0)
                return;

            if(chainsToAdd.Count > 1)
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAdd_MoreThanOne);

            if (activity.ChainTransactions.Count == 0)
                return;

            var chainToAdd = chainsToAdd.Single();
            var lastChain = this.FindLastChain(activity.ChainTransactions);

            if (chainToAdd.ParentId != lastChain.Id)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionAdd_InvalidParentId);
            }
        }

        private void ValidateRemovedChains(ActivityCommandBase message, Activity activity)
        {
            var chainsToRemove = activity.ChainTransactions
                                      .Where(x => message.ChainTransactions.Select(y => y.Id).Contains(x.Id) == false)
                                      .ToList();

            if (chainsToRemove.Count == 0)
                return;

            if(chainsToRemove.Count > 1)
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionRemove_MoreThanOne);

            var chainToAdd = chainsToRemove.Single();
            var lastChain = this.FindLastChain(activity.ChainTransactions);

            if (chainToAdd.Id != lastChain.Id)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityChainTransactionRemove_InvalidId);
            }
        }

        private ChainTransaction FindLastChain(IEnumerable<ChainTransaction> chains)
        {
            ChainTransaction firstChain = chains.First(c => c.ParentId == null);

            ChainTransaction lastChain = firstChain;
            ChainTransaction nextChain = null;
            do
            {
                nextChain = chains.SingleOrDefault(c => c.Id == lastChain.ParentId);
                if (nextChain != null)
                    lastChain = nextChain;
            }
            while (nextChain != null);

            return lastChain;
        }
    }
}
