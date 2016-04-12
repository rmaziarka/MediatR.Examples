namespace KnightFrank.Antares.Domain.Property.QueryResults
{
    using System;
    using System.Collections.Generic;

    public class PropertyTypeQuerySingleResult
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public IEnumerable<PropertyTypeQuerySingleResult> Children { get; set; }
        public int Order { get; set; }
    }
}
