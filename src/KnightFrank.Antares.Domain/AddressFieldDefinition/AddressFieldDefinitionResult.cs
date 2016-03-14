namespace KnightFrank.Antares.Domain.AddressFieldDefinition
{
    public class AddressFieldDefinitionResult
    {
        public string Name { get; set; }

        public string LabelKey { get; set; }

        public bool Required { get; set; }

        public bool Regex { get; set; }

        public int RowOrder { get; set; }

        public int ColumnOrder { get; set; }
    }
}
