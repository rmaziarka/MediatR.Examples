namespace KnightFrank.Antares.Domain.UnitTests.Resource.Queries
{
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Resource.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("ResourceLocalisedQueryDomainValidator")]
    [Trait("FeatureTitle", "Resources")]
    public class ResourceLocalisedQueryDomainValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ResourceLocalisedQueryDomainValidator_When_Validating_Then_IsoCodeShouldHaveLocaleValidator(
            ResourceLocalisedQueryDomainValidator queryDomainValidator)
        {
            queryDomainValidator.ShouldHaveChildValidator(q => q.IsoCode, typeof(LocaleValidator));
        }
    }
}
