namespace KnightFrank.Antares.Domain.UnitTests.Contact.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Contact.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("ContactQueryValidator")]
    [Trait("FeatureTitle", "Contacts")]
    public class ContactQueryValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_InvalidContactQuery_When_IdIsEmptyGuid_Then_IsNotValid(
            ContactQuery query,
            ContactQueryValidator validator)
        {
            query.Id = default(Guid);

            ValidationResult validationResult = validator.Validate(query);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.ErrorCode == nameof(Messages.notequal_error));
        }
    }
}