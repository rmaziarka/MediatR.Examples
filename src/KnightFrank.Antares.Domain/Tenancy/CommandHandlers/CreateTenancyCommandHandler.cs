namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    using MediatR;
    using Enums = Common.Enums;

    public class CreateTenancyCommandHandler : IRequestHandler<CreateTenancyCommand, Guid>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<Tenancy> tenancyGenericRepository;
        private readonly ITenancyReferenceMapper<TenancyTerm> termMapper;
        private readonly IReferenceMapper<CreateTenancyCommand, Tenancy, Contact> tenancyContactsMapper;
        private readonly IAttributeValidator<Tuple<Enums.TenancyType, Enums.RequirementType>> attributeValidator;
        private readonly IGenericRepository<Requirement> requirementRepository;
        private readonly IGenericRepository<Dal.Model.Tenancy.TenancyType> tenancyTypeRepository;

        public CreateTenancyCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<Tenancy> tenancyGenericRepository,
            ITenancyReferenceMapper<TenancyTerm> termMapper,
            IAttributeValidator<Tuple<Enums.TenancyType, Enums.RequirementType>> attributeValidator,
            IGenericRepository<Requirement> requirementRepository,
            IGenericRepository<Dal.Model.Tenancy.TenancyType> tenancyTypeRepository,
            IReferenceMapper<CreateTenancyCommand, Tenancy, Contact> tenancyContactsMapper)
        {
            this.entityValidator = entityValidator;
            this.tenancyGenericRepository = tenancyGenericRepository;
            this.termMapper = termMapper;
            this.attributeValidator = attributeValidator;
            this.requirementRepository = requirementRepository;
            this.tenancyTypeRepository = tenancyTypeRepository;
            this.tenancyContactsMapper = tenancyContactsMapper;
        }

        public Guid Handle(CreateTenancyCommand message)
        {
            Requirement requirement =
                this.requirementRepository.GetWithInclude(x => x.Id == message.RequirementId, x => x.RequirementType).FirstOrDefault();

            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            this.entityValidator.EntityExists(requirement, message.RequirementId);

            this.entityValidator.EntitiesExist<Contact>(message.LandlordContacts);
            this.entityValidator.EntitiesExist<Contact>(message.TenantContacts);

            Enums.RequirementType requirementTypeEnum = EnumExtensions.ParseEnum<Enums.RequirementType>(requirement.RequirementType.EnumCode);
            Enums.TenancyType tenancyTypeEnum = EnumMapper.GetEnum<Enums.RequirementType, Enums.TenancyType>(requirementTypeEnum);

            this.attributeValidator.Validate(PageType.Create, Tuple.Create(tenancyTypeEnum, requirementTypeEnum), message);

            Dal.Model.Tenancy.TenancyType tenancyType = this.GetTenancyType(tenancyTypeEnum);

            var tenancy = new Tenancy
            {
                RequirementId = message.RequirementId,
                ActivityId = message.ActivityId,
                TenancyTypeId = tenancyType.Id
            };

            requirement.Tenancy = tenancy;

            this.termMapper.ValidateAndAssign(message, tenancy);
            this.tenancyContactsMapper.ValidateAndAssign(message, tenancy);

            this.tenancyGenericRepository.Add(tenancy);
            this.tenancyGenericRepository.Save();

            return tenancy.Id;
        }

        private Dal.Model.Tenancy.TenancyType GetTenancyType(Enums.TenancyType tenancyTypeEnum)
        {
            return this.tenancyTypeRepository.FindBy(x => x.EnumCode == tenancyTypeEnum.ToString()).Single();
        }
    }
}