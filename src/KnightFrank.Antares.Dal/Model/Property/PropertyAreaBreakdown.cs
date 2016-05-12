namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    public class PropertyAreaBreakdown : BaseEntity
    {
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }

        public int Order { get; set; }
    }
}