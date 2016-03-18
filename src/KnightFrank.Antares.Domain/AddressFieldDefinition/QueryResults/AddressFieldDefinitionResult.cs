namespace KnightFrank.Antares.Domain.AddressFieldDefinition.QueryResults
{
    public class AddressFieldDefinitionResult
    {
        public string Name { get; set; }

        public string LabelKey { get; set; }

        public bool Required { get; set; }

        public string RegEx { get; set; }

        public short RowOrder { get; set; }

        public short ColumnOrder { get; set; }

        public short ColumnSize { get; set; }
    }
}
