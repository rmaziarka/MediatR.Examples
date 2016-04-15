namespace KnightFrank.Antares.Domain.Enum.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Enum.Queries;

    using MediatR;

    public class EnumLocalisedQueryHandler : IRequestHandler<EnumLocalisedQuery, Dictionary<Guid, string>>
    {
        private readonly IDomainValidator<EnumLocalisedQuery> domainValidator;

        private readonly IReadGenericRepository<EnumLocalised> enumLocalisedRepository;

        public EnumLocalisedQueryHandler(
            IDomainValidator<EnumLocalisedQuery> domainValidator,
            IReadGenericRepository<EnumLocalised> enumLocalisedRepository)
        {
            this.enumLocalisedRepository = enumLocalisedRepository;
            this.domainValidator = domainValidator;
        }

        public Dictionary<Guid, string> Handle(EnumLocalisedQuery query)
        {
            ValidationResult validationResult = this.domainValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            Dictionary<Guid, string> dictionary =
                this.enumLocalisedRepository.Get()
                    .Where(el => el.Locale.IsoCode == query.IsoCode)
                    .ToDictionary(el => el.EnumTypeItemId, el => el.Value);

            return dictionary;
        }
    }
}
