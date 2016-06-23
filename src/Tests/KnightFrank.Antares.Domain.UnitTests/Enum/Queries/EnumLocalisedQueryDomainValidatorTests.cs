namespace KnightFrank.Antares.Domain.UnitTests.Enum.Queries
{
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("EnumLocalisedQueryDomainValidator")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumLocalisedQueryDomainValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_EnumLocalisedQueryDomainValidator_When_Validating_Then_IsoCodeShouldHaveLocaleValidator(
            EnumLocalisedQueryDomainValidator queryDomainValidator)
        {
            queryDomainValidator.ShouldHaveChildValidator(q => q.IsoCode, typeof(LocaleValidator));
        }
    }
}
