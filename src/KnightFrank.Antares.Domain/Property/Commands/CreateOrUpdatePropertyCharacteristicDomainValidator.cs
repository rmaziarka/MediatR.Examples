namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreateOrUpdatePropertyCharacteristicDomainValidator : AbstractValidator<CreateOrUpdatePropertyCharacteristic>,
                                                                       IDomainValidator<CreateOrUpdatePropertyCharacteristic>
    {
        private readonly IGenericRepository<Characteristic> characteristicRepository;

        public CreateOrUpdatePropertyCharacteristicDomainValidator(IGenericRepository<Characteristic> characteristicRepository)
        {
            this.characteristicRepository = characteristicRepository;

            this.Custom(this.CharacteristicExists);
            this.Custom(this.CharacteristicIsEnabled);
            this.Custom(this.CharacteristicIsDisplayText);
        }

        private ValidationFailure CharacteristicExists(CreateOrUpdatePropertyCharacteristic command)
        {
            Characteristic characteristic = this.characteristicRepository.GetById(command.CharacteristicId);

            return characteristic == null
                       ? new ValidationFailure(nameof(command.CharacteristicId), "Characteristic does not exist.")
                       : null;
        }

        private ValidationFailure CharacteristicIsEnabled(CreateOrUpdatePropertyCharacteristic command)
        {
            Characteristic characteristic = this.characteristicRepository.GetById(command.CharacteristicId);

            return characteristic == null || characteristic.IsEnabled
                       ? null
                       : new ValidationFailure(nameof(command.CharacteristicId), "Characteristic is disabled.");
        }

        private ValidationFailure CharacteristicIsDisplayText(CreateOrUpdatePropertyCharacteristic command)
        {
            Characteristic characteristic = this.characteristicRepository.GetById(command.CharacteristicId);

            return !characteristic.IsDisplayText && !string.IsNullOrWhiteSpace(command.Text)
                       ? new ValidationFailure(nameof(command.Text), "Characteristic shouldn't have a text.")
                       : null;
        }
    }
}
