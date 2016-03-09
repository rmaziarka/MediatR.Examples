namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;

    public class AddressFieldLabel : BaseEntity
    {
        public virtual AddressField AddressField { get; set; }
        public Guid AddressFieldId { get; set; }
        public string LabelKey { get; set; }
        public virtual ICollection<AddressFieldDefinition> AddressFieldFormDefinitions { get; set; }
    }
}