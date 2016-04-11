namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Property;

    internal class PropertyAttributeFormConfiguration : BaseEntityConfiguration<PropertyAttributeForm>
    {
        public PropertyAttributeFormConfiguration()
        {
            this.Property(p => p.CountryId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_CountryId_PropertyTypeId", 1)
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.PropertyTypeId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_CountryId_PropertyTypeId", 2)
                    {
                        IsUnique = true
                    }));
        }
    }
}
