namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityStatusValidator")]
    public class ActivityStatusValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly IFixture fixture;
        private const string NotEmptyError = "notempty_error";

        public ActivityStatusValidatorTests()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Theory]
        [AutoMoqData]
        public void Given_ActivityStatusDoesNotExist_When_Validating_Then_IsInvalid(
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           ActivityStatusValidator validator,
           Guid activityStatusId)
        {
            // Arrange 
            enumTypeItemRepository.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(default(EnumTypeItem));

            // Act
            ValidationResult validationResult = validator.Validate(activityStatusId);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(activityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorMessage == "Activity Status does not exist.");
        }
    }
}