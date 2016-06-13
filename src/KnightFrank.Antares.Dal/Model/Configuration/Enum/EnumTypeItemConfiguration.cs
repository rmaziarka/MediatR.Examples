namespace KnightFrank.Antares.Dal.Model.Configuration.Enum
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Enum;

    internal sealed class EnumTypeItemConfiguration : BaseEntityConfiguration<EnumTypeItem>
    {
        public EnumTypeItemConfiguration()
        {
            this.Property(r => r.Code).HasMaxLength(40).IsRequired();

            var uniqueIndexName = "IX_Code_EnumTypeId";
            this.Property(x => x.Code)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(x => x.EnumTypeId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));

        }
    }
}
