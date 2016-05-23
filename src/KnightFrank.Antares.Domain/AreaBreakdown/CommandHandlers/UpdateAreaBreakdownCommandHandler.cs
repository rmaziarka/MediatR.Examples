namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class UpdateAreaBreakdownCommandHandler : IRequestHandler<UpdateAreaBreakdownCommand, Guid>
    {
        private readonly IGenericRepository<PropertyAreaBreakdown> propertyAreaBreakdownRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<Property> propertyRepository;

        public UpdateAreaBreakdownCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<PropertyAreaBreakdown> propertyAreaBreakdownRepository,
            IGenericRepository<Property> propertyRepository)
        {
            this.entityValidator = entityValidator;
            this.propertyAreaBreakdownRepository = propertyAreaBreakdownRepository;
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(UpdateAreaBreakdownCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);
            this.entityValidator.EntityExists(property, command.PropertyId);

            PropertyAreaBreakdown propertyAreaBreakdown = this.propertyAreaBreakdownRepository.GetById(command.Id);
            this.entityValidator.EntityExists(propertyAreaBreakdown, command.Id);

            if (propertyAreaBreakdown.PropertyId != property.Id)
            {
                throw new BusinessValidationException(ErrorMessage.PropertyAreaBreakdown_Is_Assigned_To_Other_Property);
            }

            Mapper.Map(command, propertyAreaBreakdown);

            this.propertyAreaBreakdownRepository.Save();

            return propertyAreaBreakdown.Id;
        }
    }
}
