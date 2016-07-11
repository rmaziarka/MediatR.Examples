namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Domain.Tenancy.Relations;

    using MediatR;
    using Enums = Common.Enums;

    public class UpdateTenancyCommandHandler : IRequestHandler<UpdateTenancyCommand, Guid>
    {
        private readonly IGenericRepository<Tenancy> tenancyRepository;
        private readonly ITenancyReferenceMapper<TenancyTerm> termMapper;
        private readonly IEntityValidator entityValidator;
        private readonly IAttributeValidator<Tuple<Enums.TenancyType, Enums.RequirementType>> attributeValidator;

        public UpdateTenancyCommandHandler(IGenericRepository<Tenancy> tenancyRepository,
            ITenancyReferenceMapper<TenancyTerm> termMapper,
            IEntityValidator entityValidator,
            IAttributeValidator<Tuple<Enums.TenancyType, Enums.RequirementType>> attributeValidator)
        {
            this.tenancyRepository = tenancyRepository;
            this.termMapper = termMapper;
            this.entityValidator = entityValidator;
            this.attributeValidator = attributeValidator;
        }

        public Guid Handle(UpdateTenancyCommand message)
        {
            Tenancy tenancy = this.tenancyRepository.GetWithInclude(
                x => x.Id == message.TenancyId,
                x => x.Requirement,
                x => x.Requirement.RequirementType,
                x => x.TenancyType,
                x => x.Terms).FirstOrDefault();

            this.entityValidator.EntityExists(tenancy, message.TenancyId);

            var requirementTypeEnum = EnumExtensions.ParseEnum<Enums.RequirementType>(tenancy.Requirement.RequirementType.EnumCode);
            var tenancyTypeEnum = EnumExtensions.ParseEnum<Enums.TenancyType>(tenancy.TenancyType.EnumCode);

            this.attributeValidator.Validate(PageType.Create, Tuple.Create(tenancyTypeEnum, requirementTypeEnum), message);

            this.termMapper.ValidateAndAssign(message, tenancy);

            this.tenancyRepository.Save();

            return tenancy.Id;
        }
    }
}