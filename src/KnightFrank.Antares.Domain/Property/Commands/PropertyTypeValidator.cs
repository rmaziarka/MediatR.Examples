using System;

namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class PropertyTypeValidator : AbstractValidator<Guid>
    {
        private readonly IGenericRepository<PropertyType> propertyTypeRepository;

        public PropertyTypeValidator(IGenericRepository<PropertyType> propertyTypeRepository)
        {
            this.propertyTypeRepository = propertyTypeRepository;

            this.Custom(this.PropertyTypeExists);
        }

        private ValidationFailure PropertyTypeExists(Guid propertyTypeId)
        {
            var propertyType = this.propertyTypeRepository.GetById(propertyTypeId);
            if (propertyType == null)
            {
                return new ValidationFailure("PropertyTypeId", "Specified property type is invalid.");
            }
            
            return null;
        }
    }
}
