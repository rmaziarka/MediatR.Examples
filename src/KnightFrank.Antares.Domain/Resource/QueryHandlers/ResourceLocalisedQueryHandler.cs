namespace KnightFrank.Antares.Domain.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Common.Specifications;
    using KnightFrank.Antares.Domain.Resource.Queries;

    using MediatR;

    public class ResourceLocalisedQueryHandler : IRequestHandler<ResourceLocalisedQuery, Dictionary<Guid, string>>
    {
        private readonly IDomainValidator<ResourceLocalisedQuery> domainValidator;

        private readonly IResourceLocalisedRepositoryProvider repositoryProvider;

        public ResourceLocalisedQueryHandler(
            IDomainValidator<ResourceLocalisedQuery> domainValidator,
            IResourceLocalisedRepositoryProvider repositoryProvider)
        {
            this.domainValidator = domainValidator;
            this.repositoryProvider = repositoryProvider;
        }

        public Dictionary<Guid, string> Handle(ResourceLocalisedQuery query)
        {
            ValidationResult validationResult = this.domainValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            return
                this.repositoryProvider.GetRepositories()
                    .Select(
                        covariantReadGenericRepository =>
                        covariantReadGenericRepository.Get().Where(new IsLocalised<ILocalised>(query.IsoCode).SatisfiedBy()).ToList())
                    .SelectMany(x => x)
                    .ToDictionary(x => x.ResourceId, x => x.Value);
        }
    }
}
