﻿namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    internal class PropertyConfiguration : BaseEntityConfiguration<Model.Property.Property>
    {
        public PropertyConfiguration()
        {
            this.HasRequired(p => p.Address).WithMany().HasForeignKey(p => p.AddressId);
        }
    }
}