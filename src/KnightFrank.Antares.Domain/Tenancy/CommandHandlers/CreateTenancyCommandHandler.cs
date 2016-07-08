namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Domain.Tenancy.Relations;

    using MediatR;

    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

    public class CreateTenancyCommandHandler : IRequestHandler<CreateTenancyCommand, Guid>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<Tenancy> tenancyGenericRepository;
        private readonly ITenancyReferenceMapper<TenancyTerm> termMapper;
        private readonly IAttributeValidator<Tuple<TenancyType>> attributeValidator;
        //private readonly IGenericRepository<TenancyType> tenancyTypeRepository;

        public CreateTenancyCommandHandler(
            IEntityValidator entityValidator, 
            IGenericRepository<Tenancy> tenancyGenericRepository, 
            ITenancyReferenceMapper<TenancyTerm> termMapper, 
            IAttributeValidator<Tuple<TenancyType>> attributeValidator)
        {
            this.entityValidator = entityValidator;
            this.tenancyGenericRepository = tenancyGenericRepository;
            this.termMapper = termMapper;
            this.attributeValidator = attributeValidator;
        }

        public Guid Handle(CreateTenancyCommand message)
        {
            // TODO implement
            //Dal.Model.Property.Activities.ActivityType activityType = this.tenancyTpeRepository.GetById(message.ActivityTypeId);
            //this.entityValidator.EntityExists(activityType, message.ActivityTypeId);
            //var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);
            //this.attributeValidator.Validate(PageType.Create, Tuple.Create(propertyTypeEnum, activityTypeEnum), message);

            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            this.entityValidator.EntityExists<Requirement>(message.RequirementId);

            this.entityValidator.EntitiesExist<Contact>(message.Landlords);
            this.entityValidator.EntitiesExist<Contact>(message.Tenants);

            var tenancy = new Tenancy
            {
                RequirementId = message.RequirementId,
                ActivityId = message.ActivityId
            };

            this.termMapper.ValidateAndAssign(message, tenancy);

            this.tenancyGenericRepository.Add(tenancy);
            this.tenancyGenericRepository.Save();

            return tenancy.Id;
        }
    }
}