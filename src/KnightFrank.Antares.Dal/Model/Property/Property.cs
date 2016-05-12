namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class Property : BaseEntity
    {
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Ownership> Ownerships { get; set; } = new List<Ownership>();

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public Guid PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public Guid DivisionId { get; set; }

        public virtual EnumTypeItem Division { get; set; }

        //TODO: column should be required - to do in US 20406
        public Guid? AttributeValuesId { get; set; }

        public virtual AttributeValues AttributeValues { get; set; }

        public virtual ICollection<PropertyCharacteristic> PropertyCharacteristics { get; set; } = new List<PropertyCharacteristic>();

        public double? TotalAreaBreakdown { get; set; }

        public virtual ICollection<PropertyAreaBreakdown> PropertyAreaBreakdowns { get; set; } = new List<PropertyAreaBreakdown>();
    }
}
