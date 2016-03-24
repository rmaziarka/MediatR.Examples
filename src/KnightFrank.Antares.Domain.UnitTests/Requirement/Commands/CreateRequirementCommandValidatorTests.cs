namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System.Collections.Generic;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Contact;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateRequirementCommandValidator")]
    [Trait("FeatureTitle", "Requirements")]
    public class CreateRequirementCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private CreateRequirementCommand command;

        public CreateRequirementCommandValidatorTests()
        {
            this.command = new CreateRequirementCommand
            {
                MinPrice = 1,
                MaxPrice = 2,
                MinArea = 1000,
                MaxArea = 2000,
                MinBathrooms = 1,
                MaxBathrooms = 2,
                MinBedrooms = 1,
                MaxBedrooms = 2,
                MinLandArea = 10000,
                MaxLandArea = 20000,
                MinParkingSpaces = 1,
                MaxParkingSpaces = 2,
                MinReceptionRooms = 1,
                MaxReceptionRooms = 2,
                Contacts = new List<ContactDto> { new ContactDto { FirstName = "Michal", Surname = "Lenartowicz", Title = "Mr" } }
            };
        }

        [Theory]
        [AutoData]
        public void Given_CorrectCreateRequirementCommand_When_Validating_Then_NoValidationErrors(CreateRequirementCommandValidator validator)
        {
            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithTooLongDescription_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.Description = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            TestIncorrectCommand(validator, this.command, nameof(this.command.Description));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxPrice), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinBedrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxBedrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBedrooms), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinReceptionRooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxReceptionRooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxReceptionRooms), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinBathrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinBathrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxBathrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBathrooms), 2);
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinParkingSpaces = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxParkingSpaces = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxParkingSpaces), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxArea), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinLandArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MaxLandArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxLandArea), 2);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceGreaterThanMaxPrice_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinPrice = 2;
            this.command.MaxPrice = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBedroomsGreaterThanMaxBedrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinBedrooms = 2;
            this.command.MaxBedrooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinReceptionRoomsGreaterThanMaxReceptionRooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinReceptionRooms = 2;
            this.command.MaxReceptionRooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsGreaterThanMaxBathrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinBathrooms = 2;
            this.command.MaxBathrooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBathrooms));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesGreaterThanMaxParkingSpaces_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinParkingSpaces = 2;
            this.command.MaxParkingSpaces = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaGreaterThanMaxArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinArea = 2;
            this.command.MaxArea = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxArea));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaGreaterThanMaxLandArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator)
        {
            this.command.MinLandArea = 2;
            this.command.MaxLandArea = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithNoContacts_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.Contacts.Clear();

            TestIncorrectCommand(validator, this.command, nameof(this.command.Contacts));
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
