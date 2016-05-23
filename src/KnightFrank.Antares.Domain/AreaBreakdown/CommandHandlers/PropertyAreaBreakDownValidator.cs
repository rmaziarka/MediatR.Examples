namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class PropertyAreaBreakdownValidator : IPropertyAreaBreakdownValidator
    {
        public void IsAssignToProperty(PropertyAreaBreakdown propertyAreaBreakdown, Property property)
        {
            if (propertyAreaBreakdown.PropertyId != property.Id)
            {
                throw new BusinessValidationException(ErrorMessage.PropertyAreaBreakdown_Is_Assigned_To_Other_Property);
            }
        }
    }
}
