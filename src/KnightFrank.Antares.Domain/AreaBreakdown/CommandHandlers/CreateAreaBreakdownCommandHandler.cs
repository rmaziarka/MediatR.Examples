namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    public class CreateAreaBreakdownCommandHandler : IRequestHandler<CreateAreaBreakdownCommand, IList<Guid>>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository;

        public CreateAreaBreakdownCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository)
        {
            this.entityValidator = entityValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.propertyRepository = propertyRepository;
            this.areaBreakdownRepository = areaBreakdownRepository;
        }

        public IList<Guid> Handle(CreateAreaBreakdownCommand command)
        {
            Property property =
                this.propertyRepository.GetWithInclude(
                    x => x.Id == command.PropertyId,
                    x => x.PropertyAreaBreakdowns).SingleOrDefault();

            this.entityValidator.EntityExists(property, command.PropertyId);
            // ReSharper disable once PossibleNullReferenceException
            this.ValidateIfCommercialProperty(property.DivisionId);

            int areaBreakdownCount = property.PropertyAreaBreakdowns.Count;

            List<PropertyAreaBreakdown> areas = command.Areas.Select(
                (x, i) =>
                new PropertyAreaBreakdown
                {
                    PropertyId = command.PropertyId,
                    Name = x.Name,
                    Size = x.Size,
                    Order = areaBreakdownCount + i
                }).ToList();

            areas.ForEach(x => this.areaBreakdownRepository.Add(x));

            this.UpdateTotalAreaBreakdown(areas, property);
            this.areaBreakdownRepository.Save();

            return areas.Select(x => x.Id).ToList();
        }

        private void ValidateIfCommercialProperty(Guid divisionId)
        {
            EnumTypeItem enumTypeItem = this.enumTypeItemRepository.GetById(divisionId);

            if (enumTypeItem == null || enumTypeItem.Code != DivisionEnum.Commercial.ToString())
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreateOnlyCommercialPropertyShouldHaveAreaBreakdownMessage());
            }
        }

        private void UpdateTotalAreaBreakdown(List<PropertyAreaBreakdown> areas, Property property)
        {
            double areaBreakdownSizeSum = areas.Sum(x => x.Size);

            if (!property.TotalAreaBreakdown.HasValue)
            {
                property.TotalAreaBreakdown = 0;
            }

            property.TotalAreaBreakdown += areaBreakdownSizeSum;
        }
    }
}