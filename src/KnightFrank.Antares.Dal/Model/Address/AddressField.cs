namespace KnightFrank.Antares.Dal.Model.Address
{
    using System.Collections.Generic;

    public class AddressField : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<AddressFieldDefinition> AddressFieldFormDefinitions { get; set; }

        public virtual ICollection<AddressFieldLabel> AddressFieldLabels { get; set; }
    }
}
