namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("CreateActivityCommandValidator")]
    public class CreateActivityCommandValidatorTests : IClassFixture<CreateActivityCommand>
    {
        private readonly IFixture fixture;
        private const string NotEmptyError = "notempty_error";

        public CreateActivityCommandValidatorTests()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateActivityCommand_When_Validating_Then_IsValid(CreateActivityValidator validator,
            CreateActivityCommand cmd)
        {
            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_PropertyDoesNotExist_When_Validating_Then_IsInvalid(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository
            , CreateActivityValidator validator, CreateActivityCommand cmd)
        {
            // Arrange 
            propertyRepository.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(default(Property));

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.PropertyId));
            propertyRepository.Verify(p => p.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandPropertyIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository
            , CreateActivityValidator validator)
        {
            // Arrange 
            CreateActivityCommand cmd = this.fixture.Build<CreateActivityCommand>().With(c => c.PropertyId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.PropertyId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ActivityStatusDoesNotExist_When_Validating_Then_IsInvalid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository
            , CreateActivityValidator validator, CreateActivityCommand cmd)
        {
            // Arrange 
            enumTypeItemRepository.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(default(EnumTypeItem));

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityStatusId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandActivityStatusIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository
            , CreateActivityValidator validator)
        {
            // Arrange 
            CreateActivityCommand cmd =
                this.fixture.Build<CreateActivityCommand>().With(c => c.ActivityStatusId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }
    }
}
