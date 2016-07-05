namespace KnightFrank.Antares.Domain.Requirement.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    using MediatR;

    public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Guid>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;
        private readonly IGenericRepository<Dal.Model.Property.RequirementType> requirementTypeRepository;
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IAddressValidator addressValidator;
        private readonly IEntityMapper<Requirement> requirementEntityMapper;
        private readonly IAttributeValidator<Tuple<RequirementType>> attributeValidator;
        private readonly IEntityValidator entityValidator;

        public CreateRequirementCommandHandler(
            IGenericRepository<Requirement> requirementRepository,
            IGenericRepository<Dal.Model.Property.RequirementType> requirementTypeRepository,
            IGenericRepository<Contact> contactRepository,
            IAddressValidator addressValidator,
            IEntityMapper<Requirement> requirementEntityMapper,
            IAttributeValidator<Tuple<RequirementType>> attributeValidator,
            IEntityValidator entityValidator)
        {
            this.requirementRepository = requirementRepository;
            this.requirementTypeRepository = requirementTypeRepository;
            this.contactRepository = contactRepository;
            this.addressValidator = addressValidator;
            this.requirementEntityMapper = requirementEntityMapper;
            this.attributeValidator = attributeValidator;
            this.entityValidator = entityValidator;
        }

        public Guid Handle(CreateRequirementCommand message)
        {                     
            Dal.Model.Property.RequirementType requirementType = this.requirementTypeRepository.GetById(message.RequirementTypeId);
            this.entityValidator.EntityExists(requirementType, message.RequirementTypeId);
            var requirementTypeEnum = EnumExtensions.ParseEnum<RequirementType>(requirementType.EnumCode);
            this.attributeValidator.Validate(PageType.Create, new Tuple<RequirementType>(requirementTypeEnum), message);

            this.addressValidator.Validate(message.Address);

            var requirement = new Requirement
            {
                RequirementTypeId = message.RequirementTypeId
            };
            
            requirement = this.requirementEntityMapper.MapAllowedValues(message, requirement, PageType.Create);
            requirement.Address = AutoMapper.Mapper.Map(message.Address, requirement.Address);            

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            if (!message.ContactIds.All(x => existingContacts.Select(c => c.Id).Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Requirement_Applicants_Id);
            }

            requirement.Contacts = existingContacts;
            requirement.CreateDate = DateTime.UtcNow;

            this.requirementRepository.Add(requirement);
            this.requirementRepository.Save();

            return requirement.Id;
        }
    }
}
