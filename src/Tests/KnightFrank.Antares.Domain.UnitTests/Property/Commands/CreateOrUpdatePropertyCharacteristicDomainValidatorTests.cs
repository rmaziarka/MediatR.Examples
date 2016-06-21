namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateOrUpdatePropertyCharacteristicDomainValidator")]
    [Trait("FeatureTitle", "Property")]
    public class CreateOrUpdatePropertyCharacteristicDomainValidatorTests
    {
        [Theory]
        [InlineAutoMoqData(true, "text")]
        [InlineAutoMoqData(false, (string)null)]
        [InlineAutoMoqData(false, "")]
        [InlineAutoMoqData(false, " ")]
        public void Given_ValidCreateOrUpdatePropertyCharacteristic_When_Validating_Then_IsValid(
            bool isDisplayText,
            string text,
            [Frozen] Mock<IGenericRepository<Characteristic>> characteristicRepository,
            CreateOrUpdatePropertyCharacteristicDomainValidator validator,
            IFixture fixture)
        {
            // Arrange
            CreateOrUpdatePropertyCharacteristic cmd =
                fixture.Build<CreateOrUpdatePropertyCharacteristic>()
                       .With(c => c.CharacteristicId, Guid.NewGuid())
                       .With(c => c.Text, text)
                       .Create();

            Characteristic characteristic =
                fixture.Build<Characteristic>().With(c => c.IsDisplayText, isDisplayText).With(c => c.IsEnabled, true).Create();

            characteristicRepository.Setup(r => r.GetById(cmd.CharacteristicId)).Returns(characteristic);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
            characteristicRepository.Verify(r => r.GetById(cmd.CharacteristicId), Times.Exactly(3));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CharacteristicNotExist_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateOrUpdatePropertyCharacteristic cmd,
            [Frozen] Mock<IGenericRepository<Characteristic>> characteristicRepository,
            CreateOrUpdatePropertyCharacteristicDomainValidator validator)
        {
            // Arrange
            characteristicRepository.Setup(r => r.GetById(cmd.CharacteristicId)).Returns((Characteristic)null);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.CharacteristicId));
            result.Errors.Should().Contain(e => e.ErrorCode == "characteristicid_notexists");
            characteristicRepository.Verify(r => r.GetById(cmd.CharacteristicId), Times.Exactly(3));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CharacteristicIsDisabled_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateOrUpdatePropertyCharacteristic cmd,
            [Frozen] Mock<IGenericRepository<Characteristic>> characteristicRepository,
            CreateOrUpdatePropertyCharacteristicDomainValidator validator,
            IFixture fixture)
        {
            // Arrange
            Characteristic characteristic = fixture.Build<Characteristic>().With(c => c.IsEnabled, false).Create();
            characteristicRepository.Setup(r => r.GetById(cmd.CharacteristicId)).Returns(characteristic);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.CharacteristicId));
            result.Errors.Should().Contain(e => e.ErrorCode == "characteristicid_isdisabled");
            characteristicRepository.Verify(r => r.GetById(cmd.CharacteristicId), Times.Exactly(3));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CharacteristicNoneDisplayText_When_ValidatingWithText_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateOrUpdatePropertyCharacteristic cmd,
            [Frozen] Mock<IGenericRepository<Characteristic>> characteristicRepository,
            CreateOrUpdatePropertyCharacteristicDomainValidator validator,
            IFixture fixture)
        {
            // Arrange
            Characteristic characteristic = fixture.Build<Characteristic>().With(c=>c.IsDisplayText, false).With(c => c.IsEnabled, true).Create();
            characteristicRepository.Setup(r => r.GetById(cmd.CharacteristicId)).Returns(characteristic);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Text));
            result.Errors.Should().Contain(e => e.ErrorCode == "characteristictext_shouldbeempty");
            characteristicRepository.Verify(r => r.GetById(cmd.CharacteristicId), Times.Exactly(3));
        }
    }
}
