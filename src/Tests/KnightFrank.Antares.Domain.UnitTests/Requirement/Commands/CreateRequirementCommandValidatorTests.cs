namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Requirement.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("CreateRequirementCommandValidator")]
    [Trait("FeatureTitle", "Requirements")]
    public class CreateRequirementCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateRequirementCommand command;
        private readonly CreateRequirementCommandValidator validator;

        public CreateRequirementCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();

            this.command = fixture.Build<CreateRequirementCommand>()
                               .With(x => x.MinPrice, 1)
                               .With(x => x.MaxPrice, 2)
                               .With(x => x.MinArea, 1)
                               .With(x => x.MaxArea, 2)
                               .With(x => x.MinBathrooms, 1)
                               .With(x => x.MaxBathrooms, 2)
                               .With(x => x.MinBedrooms, 1)
                               .With(x => x.MaxBedrooms, 2)
                               .With(x => x.MinLandArea, 1)
                               .With(x => x.MaxLandArea, 2)
                               .With(x => x.MinParkingSpaces, 1)
                               .With(x => x.MaxParkingSpaces, 2)
                               .With(x => x.MinReceptionRooms, 1)
                               .With(x => x.MaxReceptionRooms, 2)
                               .With(x => x.ContactIds, new List<Guid> { fixture.Create<Guid>() })
                               .With(x => x.Address, fixture.Create<CreateOrUpdateAddress>())
                               .Create();

            this.validator = fixture.Create<CreateRequirementCommandValidator>();
        }

        [Fact]
        public void Given_CorrectCreateRequirementCommand_When_Validating_Then_NoValidationErrors()
        {
            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithTooLongDescription_When_Validating_Then_ValidationErrors()
        {
            this.command.Description = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.Description));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinPrice = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinPrice));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxPriceLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxPrice = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxPrice), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWitMinBedroomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinBedrooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinBedrooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxBedroomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxBedrooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxBedrooms), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWitMinReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinReceptionRooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinReceptionRooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxReceptionRooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxReceptionRooms), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinBathrooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinBathrooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxBathroomsLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxBathrooms = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxBathrooms), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinParkingSpaces = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinParkingSpaces));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxParkingSpaces = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxParkingSpaces), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinArea = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinArea));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxAreaLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxArea = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxArea), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MinLandArea = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MinLandArea));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMaxLandAreaLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.MaxLandArea = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxLandArea), 2);
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceGreaterThanMaxPrice_When_Validating_Then_ValidationErrors()
        {
            this.command.MinPrice = 2;
            this.command.MaxPrice = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxPrice));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinBedroomsGreaterThanMaxBedrooms_When_Validating_Then_ValidationErrors()
        {
            this.command.MinBedrooms = 2;
            this.command.MaxBedrooms = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxBedrooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinReceptionRoomsGreaterThanMaxReceptionRooms_When_Validating_Then_ValidationErrors()
        {
            this.command.MinReceptionRooms = 2;
            this.command.MaxReceptionRooms = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxReceptionRooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsGreaterThanMaxBathrooms_When_Validating_Then_ValidationErrors()
        {
            this.command.MinBathrooms = 2;
            this.command.MaxBathrooms = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxBathrooms));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesGreaterThanMaxParkingSpaces_When_Validating_Then_ValidationErrors()
        {
            this.command.MinParkingSpaces = 2;
            this.command.MaxParkingSpaces = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxParkingSpaces));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaGreaterThanMaxArea_When_Validating_Then_ValidationErrors()
        {
            this.command.MinArea = 2;
            this.command.MaxArea = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxArea));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaGreaterThanMaxLandArea_When_Validating_Then_ValidationErrors()
        {
            this.command.MinLandArea = 2;
            this.command.MaxLandArea = 1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.MaxLandArea));
        }

        [Fact]
        public void Given_IncorrectCreateRequirementCommandWithNoContactIds_When_Validating_Then_ValidationErrors()
        {
            this.command.ContactIds.Clear();

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.ContactIds));
        }

        // ReSharper disable once UnusedParameter.Local
        private static void TestIncorrectCommand(CreateRequirementCommandValidator validator, CreateRequirementCommand command, string testedPropertyName, int expectedErrorCount = 1)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
            Assert.Equal(expectedErrorCount, validationResult.Errors.Count);
            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
