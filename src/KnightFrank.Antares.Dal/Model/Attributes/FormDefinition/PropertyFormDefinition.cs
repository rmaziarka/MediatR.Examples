namespace KnightFrank.Antares.Dal.Model.Attributes.FormDefinition
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyFormDefinition : FormDefinition
    {
        public Guid PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public virtual Country Country { get; set; }
    }
}
