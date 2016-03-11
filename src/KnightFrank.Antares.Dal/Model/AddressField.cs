namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;

    public class AddressField : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<AddressFieldDefinition> AddressFieldFormDefinitions { get; set; }
        public virtual ICollection<AddressFieldLabel> AddressFieldLabels { get; set; }
    }
}