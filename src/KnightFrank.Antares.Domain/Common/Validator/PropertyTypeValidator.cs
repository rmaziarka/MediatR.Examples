namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

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
            bool propertyTypeExists = this.propertyTypeRepository.Any(x => x.Id.Equals(propertyTypeId));
            return propertyTypeExists ? null : new ValidationFailure("PropertyTypeId", "PropertyType does not exist.") { ErrorCode = "propertytypeid_error" };
        }
    }
}
