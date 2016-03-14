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
                MaxReceptionRooms = 2
            };
        }

        [Theory]
        [AutoData]
        public void Given_CorrectCreateRequirementCommand_When_Validating_Then_NoValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithTooLongDescription_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.Description = string.Join(string.Empty, fixture.CreateMany<char>(4001));

            TestIncorrectCommand(validator, this.command, nameof(this.command.Description));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinBedrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBedroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxBedrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWitMinReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinReceptionRooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxReceptionRoomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxReceptionRooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinBathrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinBathrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxBathroomsLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxBathrooms = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBathrooms));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinParkingSpaces = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxParkingSpacesLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxParkingSpaces = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinLandArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MinLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMaxLandAreaLessThanZero_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MaxLandArea = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxLandArea));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinPriceGreaterThanMaxPrice_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinPrice = 2;
            this.command.MaxPrice = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxPrice));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBedroomsGreaterThanMaxBedrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinBedrooms = 2;
            this.command.MaxBedrooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBedrooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinReceptionRoomsGreaterThanMaxReceptionRooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinReceptionRooms = 2;
            this.command.MaxReceptionRooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxReceptionRooms));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinBathroomsGreaterThanMaxBathrooms_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinBathrooms = 2;
            this.command.MaxBathrooms = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxBathrooms));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinParkingSpacesGreaterThanMaxParkingSpaces_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinParkingSpaces = 2;
            this.command.MaxParkingSpaces = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxParkingSpaces));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinAreaGreaterThanMaxArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinArea = 2;
            this.command.MaxArea = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxArea));
        }


        [Theory]
        [AutoData]
        public void Given_IncorrectCreateRequirementCommandWithMinLandAreaGreaterThanMaxLandArea_When_Validating_Then_ValidationErrors(CreateRequirementCommandValidator validator, Fixture fixture)
        {
            this.command.MinLandArea = 2;
            this.command.MaxLandArea = 1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.MaxLandArea));
        }

        private static void TestIncorrectCommand(CreateRequirementCommandValidator validator, CreateRequirementCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
