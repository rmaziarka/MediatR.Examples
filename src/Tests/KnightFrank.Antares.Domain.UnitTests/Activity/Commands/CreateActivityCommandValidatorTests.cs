namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    using FixtureExtension;

    using FluentValidation.Resources;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("CreateActivityCommandValidator")]
    public class CreateActivityCommandValidatorTests : IClassFixture<CreateActivityCommand>
    {
        private readonly IFixture fixture;
        private const string NotEmptyError = "notempty_error";

        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly Mock<IGenericRepository<ActivityType>> activityTypeRepository;
        private readonly Mock<IGenericRepository<ActivityTypeDefinition>> activityTypeDefinitionRepository;
        private readonly Mock<IGenericRepository<Property>> propertyRepository;
        private readonly CreateActivityCommandValidator validator;

        public CreateActivityCommandValidatorTests()
        {
            this.fixture = new Fixture().Customize();

            this.enumTypeItemRepository = this.fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();
            this.activityTypeRepository = this.fixture.Freeze<Mock<IGenericRepository<ActivityType>>>();
            this.activityTypeDefinitionRepository = this.fixture.Freeze<Mock<IGenericRepository<ActivityTypeDefinition>>>();
            this.propertyRepository = this.fixture.Freeze<Mock<IGenericRepository<Property>>>();

            this.activityTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityType, bool>>>())).Returns(true);
            this.enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            this.activityTypeDefinitionRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                .Returns(true);
            this.propertyRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(new Property());

            this.validator = this.fixture.Create<CreateActivityCommandValidator>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateActivityCommand_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            CreateActivityCommand cmd)
        {
            // Arrange
            IEnumerable<Contact> fakeContactResult = cmd.ContactIds.Select(activityContact => new Contact { Id = activityContact });

            contactRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(fakeContactResult);

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyContactList_When_Validating_Then_IsValid(CreateActivityCommand cmd)
        {
            cmd.ContactIds = new List<Guid>();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandPropertyIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode()
        {
            // Arrange
            CreateActivityCommand cmd = this.fixture.Build<CreateActivityCommand>().With(c => c.PropertyId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.PropertyId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandActivityStatusIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode()
        {
            // Arrange
            CreateActivityCommand cmd =
                this.fixture.Build<CreateActivityCommand>().With(c => c.ActivityStatusId, default(Guid)).Create();

            this.enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(false);

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandActivityTypeIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode()
        {
            // Arrange
            CreateActivityCommand cmd =
                this.fixture.Build<CreateActivityCommand>().With(c => c.ActivityTypeId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityTypeId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandNegotiatorIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(Guid leadNegotiatorId)
        {
            // Arrange
            CreateActivityCommand cmd = this.fixture.Build<CreateActivityCommand>().With(c => c.LeadNegotiatorId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.LeadNegotiatorId), nameof(Messages.notempty_error));
        }
    }
}
