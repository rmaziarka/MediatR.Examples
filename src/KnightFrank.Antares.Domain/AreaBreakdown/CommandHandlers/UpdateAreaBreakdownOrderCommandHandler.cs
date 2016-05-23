namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;
    public class UpdateAreaBreakdownOrderCommandHandler : IRequestHandler<UpdateAreaBreakdownOrderCommand, Guid>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository;
        private readonly IGenericRepository<Property> propertyRepository;

        public UpdateAreaBreakdownOrderCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository,
            IGenericRepository<Property> propertyRepository)
        {
            this.entityValidator = entityValidator;
            this.areaBreakdownRepository = areaBreakdownRepository;
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(UpdateAreaBreakdownOrderCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);
            this.entityValidator.EntityExists(property, command.PropertyId);

            PropertyAreaBreakdown areaBreakdown = this.areaBreakdownRepository.GetById(command.AreaId);
            this.entityValidator.EntityExists(areaBreakdown, command.AreaId);

            if (areaBreakdown.PropertyId != property.Id)
            {
                throw new BusinessValidationException(ErrorMessage.PropertyAreaBreakdown_Is_Assigned_To_Other_Property);
            }

            IEnumerable<PropertyAreaBreakdown> orderedAreaBreakdownItems = this.GetOrderedAreaBreakdownItems(command, areaBreakdown);
            property.TotalAreaBreakdown = orderedAreaBreakdownItems.Sum(x => x.Size);

            this.areaBreakdownRepository.Save();

            return areaBreakdown.Id;
        }
        
        private IEnumerable<PropertyAreaBreakdown> GetOrderedAreaBreakdownItems(UpdateAreaBreakdownOrderCommand command, PropertyAreaBreakdown updatedAreaBreakdown)
        {
            List<PropertyAreaBreakdown> areaBreakdownItems =
                this.areaBreakdownRepository.FindBy(x => x.PropertyId == command.PropertyId && x.Id != command.AreaId)
                    .OrderBy(x => x.Order)
                    .ToList();

            if (command.Order > areaBreakdownItems.Count)
            {
                throw new BusinessValidationException(ErrorMessage.PropertyAreaBreakdown_OrderOutOfRange);
            }

            areaBreakdownItems.Insert(command.Order, updatedAreaBreakdown);

            foreach (var items in areaBreakdownItems.Select((x, i) => new { Value = x, Index = i }))
            {
                items.Value.Order = items.Index;
            }
            
            return areaBreakdownItems;
        }
    }
}