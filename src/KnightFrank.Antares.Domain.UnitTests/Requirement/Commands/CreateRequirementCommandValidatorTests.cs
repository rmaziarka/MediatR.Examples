namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using FluentValidation.Results;
    
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateRequirementCommandValidator")]
    [Trait("FeatureTitle", "Requirements")]
    public class CreateRequirementCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoData]
        public void Given_CorrectCreateRequirementCommand_When_Validating_Then_NoValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            var command = new CreateRequirementCommand();

            ValidationResult validationResult = validator.Validate(command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithTooLongDescription_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.Description = string.Join(string.Empty, fixture.CreateMany<string>(4000));

            TestIncorrectCommand(validator, command, nameof(command.Description));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinPrice = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxPrice = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinBedrooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxBedrooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinReceptionRooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxReceptionRooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinBathrooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinBathrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxBathrooms = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxBathrooms));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinParkingSpaces = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxParkingSpaces = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinArea = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxArea = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinLandArea = -1;

            TestIncorrectCommand(validator, command, nameof(command.MinLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MaxLandArea = -1;

            TestIncorrectCommand(validator, command, nameof(command.MaxLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceGreaterThanMaxPrice_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinPrice = 2;
            command.MaxPrice = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBedroomsGreaterThanMaxBedrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinBedrooms = 2;
            command.MaxBedrooms = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinReceptionRoomsGreaterThanMaxReceptionRooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinReceptionRooms = 2;
            command.MaxReceptionRooms = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsGreaterThanMaxBathrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinBathrooms = 2;
            command.MaxBathrooms = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxBathrooms));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesGreaterThanMaxParkingSpaces_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinParkingSpaces = 2;
            command.MaxParkingSpaces = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaGreaterThanMaxArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinArea = 2;
            command.MaxArea = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxArea));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaGreaterThanMaxLandArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, CreateRequirementCommand command, Fixture fixture)
        {
            command.MinLandArea = 2;
            command.MaxLandArea = 1;

            TestIncorrectCommand(validator, command, nameof(command.MaxLandArea));
        }

        private static void TestIncorrectCommand(CreateRequirementCommandValidator validator, CreateRequirementCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
