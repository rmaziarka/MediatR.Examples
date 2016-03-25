namespace KnightFrank.Antares.Domain.Property.QueryResults
{
    using System;

    public class PropertyTypeQuerySingleResult
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
