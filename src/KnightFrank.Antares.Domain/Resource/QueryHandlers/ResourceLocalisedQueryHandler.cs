namespace KnightFrank.Antares.Domain.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Resource.Queries;

    using MediatR;

    public class ResourceLocalisedQueryHandler : IRequestHandler<ResourceLocalisedQuery, Dictionary<Guid, string>>
    {
        private readonly IDomainValidator<ResourceLocalisedQuery> domainValidator;

        public ResourceLocalisedQueryHandler(IDomainValidator<ResourceLocalisedQuery> domainValidator)
        {
            this.domainValidator = domainValidator;
        }

        public Dictionary<Guid, string> Handle(ResourceLocalisedQuery query)
        {
            ValidationResult validationResult = this.domainValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var dictionary = new Dictionary<Guid, string>();

            dictionary.Add(Guid.Empty, "test");

            return dictionary;
        }
    }
}
