namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;
    public class CreateAreaBreakdownCommandHandler : IRequestHandler<CreateAreaBreakdownCommand, IList<Guid>>
    {
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<Property> propertyRepository;

        public CreateAreaBreakdownCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository)
        {
            this.entityValidator = entityValidator;
            this.propertyRepository = propertyRepository;
        }

        public IList<Guid> Handle(CreateAreaBreakdownCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);

            this.entityValidator.EntityExists(property, command.PropertyId);
            
            return new List<Guid>();
        }
    }
}