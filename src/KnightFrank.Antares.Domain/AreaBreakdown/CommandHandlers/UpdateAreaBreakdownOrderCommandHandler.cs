namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;
    public class UpdateAreaBreakdownOrderCommandHandler : IRequestHandler<UpdateAreaBreakdownCommand, Guid>
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

        public Guid Handle(UpdateAreaBreakdownCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);
            PropertyAreaBreakdown areaBreakdown = this.areaBreakdownRepository.GetById(command.AreaId);

            this.entityValidator.EntityExists(property, command.PropertyId);
            this.entityValidator.EntityExists(areaBreakdown, command.AreaId);

            IEnumerable<PropertyAreaBreakdown> orderedAreaBreakdownItems = this.GetOrderedAreaBreakdownItems(command, areaBreakdown);

            Mapper.Map(command, areaBreakdown);

            property.TotalAreaBreakdown = orderedAreaBreakdownItems.Sum(x => x.Size);

            this.areaBreakdownRepository.Save();

            return areaBreakdown.Id;
        }

        private IEnumerable<PropertyAreaBreakdown> GetOrderedAreaBreakdownItems(UpdateAreaBreakdownCommand command, PropertyAreaBreakdown updatedAreaBreakdown)
        {
            List<PropertyAreaBreakdown> areaBreakdownItems =
                this.areaBreakdownRepository.FindBy(x => x.PropertyId == command.PropertyId)
                    .OrderBy(x => x.Order)
                    .ToList();

            if (command.Order != updatedAreaBreakdown.Order)
            {
                areaBreakdownItems.Remove(updatedAreaBreakdown);
                areaBreakdownItems.Insert(command.Order, updatedAreaBreakdown);

                foreach (var items in areaBreakdownItems.Select((x, i) => new { Value = x, Index = i }))
                {
                    items.Value.Order = items.Index;
                }
            }

            return areaBreakdownItems;
        }
    }
}