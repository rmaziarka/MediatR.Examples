namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Property;

    public interface IPropertyAreaBreakdownValidator
    {
        void IsAssignToProperty(PropertyAreaBreakdown propertyAreaBreakdown, Property property);
    }
}
