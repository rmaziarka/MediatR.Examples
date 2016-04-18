namespace KnightFrank.Antares.Domain.Resource.Dictionaries
{
    using System;
    using System.Collections.Generic;

    public interface IResourceLocalisedDictionary
    {
        Dictionary<Guid, string> GetDictionary(string isoCode);
    }
}
