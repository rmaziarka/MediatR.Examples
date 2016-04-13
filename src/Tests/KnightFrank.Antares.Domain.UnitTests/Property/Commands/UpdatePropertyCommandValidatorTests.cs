namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("UpdatePropertyCommandValidator")]
    [Trait("FeatureTitle", "Property")]
    public class UpdatePropertyCommandValidatorTests
    {
        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly UpdatePropertyCommand command;
        private readonly UpdatePropertyCommandValidator validator;
        
        public UpdatePropertyCommandValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.command = fixture.Build<UpdatePropertyCommand>()
                .With(p => p.Address, new CreateOrUpdatePropertyAddress())
                .With(p => p.PropertyTypeId, Guid.NewGuid())
                .With(p => p.Division, new EnumTypeItem { Code = fixture.Create<string>() })
                .Create();

            this.enumTypeItemRepository = fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();

            this.validator = fixture.Create<UpdatePropertyCommandValidator>();
        }

        [Fact]
        public void Given_InvalidUpdatePropertyCommand_When_AddressIsNull_Then_TheObjectShouldBeInValidationResultWithReqExError()
        {
            // Arrange
            var updatePropertyCommand = new UpdatePropertyCommand();

            // Act
            ValidationResult validationResult = this.validator.Validate(updatePropertyCommand);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(updatePropertyCommand.Address) && e.ErrorCode == "notnull_error");
        }

        [Fact]
        public void Given_ValidUpdatePropertyCommand_When_DivisionExists_Then_NoError()
        {
            // Arrange
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_ValidUpdatePropertyCommand_When_DivisionNotExists_Then_Error()
        {
            // Arrange
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(false);

            // Act
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}