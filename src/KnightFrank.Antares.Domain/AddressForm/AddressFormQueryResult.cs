namespace KnightFrank.Antares.Domain.AddressForm
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AddressFieldDefinition;
    using KnightFrank.Antares.Domain.AddressFieldDefinition.QueryResults;

    public class AddressFormQueryResult
    {
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        public IList<AddressFieldDefinitionResult> AddressFieldDefinitions { get; set; } = new List<AddressFieldDefinitionResult>();
    }
}
