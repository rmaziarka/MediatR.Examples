namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.Validator;

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
        public void Given_ValidCreateActivityCommand_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
            CreateActivityCommandValidator validator,
            CreateActivityCommand cmd)
        {
            // Arrange 
            IEnumerable<Contact> fakeContactResult = cmd.ContactIds.Select(activityContact => new Contact { Id = activityContact });

            enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            contactRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(fakeContactResult);
            activityTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityType, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_PropertyDoesNotExist_When_Validating_Then_IsInvalid(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            CreateActivityCommandValidator validator,
            CreateActivityCommand cmd)
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
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            CreateActivityCommandValidator validator)
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
        public void Given_CommandActivityStatusIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IReadGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateActivityCommandValidator validator)
        {
            // Arrange 
            CreateActivityCommand cmd =
                this.fixture.Build<CreateActivityCommand>().With(c => c.ActivityStatusId, default(Guid)).Create();
            var fakeActivityStatus = new EnumTypeItem
            {
                Id = cmd.ActivityStatusId,
                EnumType = new EnumType { Code = "ActivityStatus" }
            };
            enumTypeItemRepository.Setup(r => r.Get()).Returns(new List<EnumTypeItem> { fakeActivityStatus }.AsQueryable());

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandActivityTypeIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
            CreateActivityCommandValidator validator)
        {
            // Arrange 
            CreateActivityCommand cmd =
                this.fixture.Build<CreateActivityCommand>().With(c => c.ActivityTypeId, default(Guid)).Create();
            
            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityTypeId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandActivityTypeDoesNotExist_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
           [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
           CreateActivityCommandValidator validator,
           CreateActivityCommand cmd)
        {
            // Arrange 
            activityTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityType, bool>>>())).Returns(false);

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.ActivityTypeId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidationRules_When_Configuring_Then_ActivityStatusIdHaveValidatorSetup(CreateActivityCommandValidator validator)
        {
            validator.ShouldHaveChildValidator(x => x.ActivityStatusId, typeof(ActivityStatusValidator));
        }
    }
}
