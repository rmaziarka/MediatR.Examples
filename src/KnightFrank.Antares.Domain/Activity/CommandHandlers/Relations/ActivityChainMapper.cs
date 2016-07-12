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

    public class ActivityChainMapper : IActivityReferenceMapper<ChainTransaction>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<ChainTransaction> chainTransactionRepository;

        public ActivityChainMapper(
            IEntityValidator entityValidator,
            IGenericRepository<ChainTransaction> chainTransactionRepository)
        {
            this.entityValidator = entityValidator;
            this.chainTransactionRepository = chainTransactionRepository;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            this.Validate(message);

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

        private void Validate(ActivityCommandBase message)
        {
            if (message.ChainTransactions.Count(x => x.IsEnd) > 1)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_ChainTransactions_IsEndCountMoreThenOne);
            }

            foreach (ChainTransaction chainTransaction in message.ChainTransactions)
            {
                this.entityValidator.EntityExists<Property>(chainTransaction.PropertyId);
                this.entityValidator.EntityExists<Company>(chainTransaction.SolicitorCompanyId);
                this.entityValidator.EntityExists<Contact>(chainTransaction.SolicitorContactId);
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
        }
    }
}
