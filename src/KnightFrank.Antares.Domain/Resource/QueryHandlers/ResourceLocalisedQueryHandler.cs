namespace KnightFrank.Antares.Domain.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Resource.Dictionaries;
    using KnightFrank.Antares.Domain.Resource.Queries;

    using MediatR;

    public class ResourceLocalisedQueryHandler : IRequestHandler<ResourceLocalisedQuery, Dictionary<Guid, string>>
    {
        private readonly IResourceLocalisedDictionary[] dictionaries;

        private readonly IDomainValidator<ResourceLocalisedQuery> domainValidator;

        public ResourceLocalisedQueryHandler(
            IDomainValidator<ResourceLocalisedQuery> domainValidator,
            IResourceLocalisedDictionary[] dictionaries)
        {
            this.domainValidator = domainValidator;
            this.dictionaries = dictionaries;
        }

        public Dictionary<Guid, string> Handle(ResourceLocalisedQuery query)
        {
            ValidationResult validationResult = this.domainValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var result = new Dictionary<Guid, string>();

            foreach (IResourceLocalisedDictionary resourceLocalisedDictionary in this.dictionaries)
            {
                Dictionary<Guid, string> dictionary = resourceLocalisedDictionary.GetDictionary(query.IsoCode);
                result = result.Concat(dictionary).ToDictionary(x => x.Key, x => x.Value);
            }

            return result;
        }
    }
}
