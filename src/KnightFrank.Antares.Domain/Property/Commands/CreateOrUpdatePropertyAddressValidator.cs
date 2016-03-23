namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;

    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Results;
    using FluentValidation.Validators;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateOrUpdatePropertyAddressValidator : AbstractValidator<CreateOrUpdatePropertyAddress>
    {
        private readonly IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository;

        public CreateOrUpdatePropertyAddressValidator(IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository)
        {
            this.addressFieldDefinitionRepository = addressFieldDefinitionRepository;
        }

        public override ValidationResult Validate(ValidationContext<CreateOrUpdatePropertyAddress> context)
        {
            CreateOrUpdatePropertyAddress address = context.InstanceToValidate;
            IEnumerable<AddressFieldDefinition> addressFieldDefinitions =
                this.addressFieldDefinitionRepository.GetWithInclude(afd => afd.AddressFormId == address.AddressFormId, afd => afd.AddressForm,
                    adf => adf.AddressField).ToList();

            // TODO validate against country 
            var addressFields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(address.City), address.City),
                new KeyValuePair<string, string>(nameof(address.PropertyName), address.PropertyName),
                new KeyValuePair<string, string>(nameof(address.PropertyNumber), address.PropertyNumber),
                new KeyValuePair<string, string>(nameof(address.Line1), address.Line1),
                new KeyValuePair<string, string>(nameof(address.Line2), address.Line2),
                new KeyValuePair<string, string>(nameof(address.Line3), address.Line3),
                new KeyValuePair<string, string>(nameof(address.Postcode), address.Postcode),
                new KeyValuePair<string, string>(nameof(address.County), address.County)
            };
                                                              
            var joinResult = addressFieldDefinitions.Join(addressFields, p => p.AddressField.Name, pi => pi.Key, (p, pi) => new { AddressFieldDefinition = p, Value = pi.Value }).ToList();
            var validationResult = new List<ValidationResult>();

            foreach (var field in joinResult)
            {
                if (field.AddressFieldDefinition.Required)
                {
                    var validator = new CustomStringValidator();
                    validator.RuleFor(el => el).NotEmpty().WithName(field.AddressFieldDefinition.AddressField.Name);
                    validationResult.Add(validator.Validate(field.Value));
                }

                if (!string.IsNullOrWhiteSpace(field.AddressFieldDefinition.RegEx))
                {
                    var validator = new CustomStringValidator();
                    validator.RuleFor(el => el).Matches(new Regex(field.AddressFieldDefinition.RegEx)).WithName(field.AddressFieldDefinition.AddressField.Name);
                    validationResult.Add(validator.Validate(field.Value));
                }

            }

            return new ValidationResult(validationResult.SelectMany(x => x.Errors));
        }
    }
}