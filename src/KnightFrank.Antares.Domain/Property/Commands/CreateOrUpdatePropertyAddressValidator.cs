namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Helpers.Extentions;

    public class CreateOrUpdatePropertyAddressValidator : AbstractValidator<CreateOrUpdatePropertyAddress>
    {
        private readonly IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository;
        private readonly IGenericRepository<AddressForm> addressFormRepository;

        public CreateOrUpdatePropertyAddressValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository)
        {
            this.RuleFor(x => x.CountryId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.AddressFormId).NotEqual(Guid.Empty).NotNull();

            this.addressFieldDefinitionRepository = addressFieldDefinitionRepository;
            this.addressFormRepository = addressFormRepository;
        }

        public override ValidationResult Validate(ValidationContext<CreateOrUpdatePropertyAddress> context)
        {
            CreateOrUpdatePropertyAddress address = context.InstanceToValidate;
            List<AddressFieldDefinition> addressFieldDefinitions = 
                this.addressFieldDefinitionRepository.GetWithInclude(
                    afd => afd.AddressFormId == address.AddressFormId, 
                    afd => afd.AddressForm,
                    adf => adf.AddressField).ToList();

            AddressForm addressForm = this.addressFormRepository.GetById(address.AddressFormId);

            if (addressForm == null)
            {
                var result = new ValidationResult();
                result.Errors.Add(new ValidationFailure(nameof(address.AddressFormId), "Address form does not exist"));

                return result;
            }

            Dictionary<string, string> addressFields = ConvertQueryToAddressFields(address);

            ValidationResult validateAgainstCountryConsistency = ValidateAgainstCountryConsistency(address, addressForm);
            ValidationResult validationAgainstAdditionalData = ValidateIfQueryHasMoreDataThenConfigured(addressFields, addressFieldDefinitions);
            ValidationResult validationAgainstConfiguration = ValidateQueryAgainstConfiguration(addressFieldDefinitions, addressFields);

            return validateAgainstCountryConsistency
                .Merge(validationAgainstAdditionalData)
                .Merge(validationAgainstConfiguration);
        }

        private static ValidationResult ValidateAgainstCountryConsistency(CreateOrUpdatePropertyAddress address, AddressForm addressForm)
        {
            var validationResult = new ValidationResult();

            if (address.CountryId != addressForm.CountryId)
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(address.CountryId), "Inconsistent data"));
            }

            return validationResult;
        }

        private static Dictionary<string, string> ConvertQueryToAddressFields(CreateOrUpdatePropertyAddress address)
        {
            List<PropertyInfo> propertyInfos = address.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();

            var addressFields = new Dictionary<string, string>();

            propertyInfos.ForEach(pi => { addressFields.Add(pi.Name, pi.GetValue(address) as string); });

            return addressFields;
        }

        private static ValidationResult ValidateQueryAgainstConfiguration(List<AddressFieldDefinition> addressFieldDefinitions, Dictionary<string, string> addressFields)
        {
            var joinResult =
                addressFieldDefinitions.Join(addressFields, p => p.AddressField.Name, pi => pi.Key,
                    (p, pi) => new { AddressFieldDefinition = p, Value = pi.Value }).ToList();

            bool isInconsistencyBetweenQueryAndDb = joinResult.Count != addressFieldDefinitions.Count;

            if (isInconsistencyBetweenQueryAndDb)
            {
                var result = new ValidationResult();
                result.Errors.Add(new ValidationFailure("address", "Invalid configuration for address"));

                return result;
            }

            List<ValidationResult> requiredValidationResults = joinResult
                .Where(f => f.AddressFieldDefinition.Required)
                .Select(f => ValidateIfFieldIsNotEmpty(f.AddressFieldDefinition.AddressField.Name, f.Value))
                .ToList();

            List<ValidationResult> regExValidationResults = joinResult
                .Where(f => !string.IsNullOrWhiteSpace(f.AddressFieldDefinition.RegEx))
                .Select(f => ValidateIfFieldMatchesRegEx(f.AddressFieldDefinition.AddressField.Name, f.Value, f.AddressFieldDefinition.RegEx))
                .ToList();

            return requiredValidationResults
                .Union(regExValidationResults)
                .Aggregate(new ValidationResult(), (result, otherResult) => result.Merge(otherResult));
        }

        private static ValidationResult ValidateIfQueryHasMoreDataThenConfigured(Dictionary<string, string> addressFields, List<AddressFieldDefinition> addressFieldDefinitions)
        {
            var fieldsWithDefinition = from addressField in addressFields.Where(x => !string.IsNullOrWhiteSpace(x.Value))
                             join addressFieldDefinition in addressFieldDefinitions on addressField.Key equals
                                 addressFieldDefinition.AddressField.Name into result
                             from r in result.DefaultIfEmpty()
                             select new { AddressFieldDefinition = r, Field = new { addressField.Value, addressField.Key } };

            List<ValidationResult> validationResults = fieldsWithDefinition
                .Where(x => x.AddressFieldDefinition == null)
                .Select(x => ValidateIfFieldIsNotRedundant(x.Field.Key, x.Field.Value))
                .ToList();

            return new ValidationResult(validationResults.SelectMany(x => x.Errors));
        }

        private static ValidationResult ValidateIfFieldIsNotRedundant(string fieldName, string fieldValue)
        {
            var validator = new CustomDynamicValidator<string>();

            validator.RuleFor(el => el).Empty().WithName(fieldName);
            ValidationResult validate = validator.Validate(fieldValue);
            return validate;
        }

        private static ValidationResult ValidateIfFieldMatchesRegEx(string fieldName, string fieldValue, string regEx)
        {
            var validator = new CustomDynamicValidator<string>();

            validator.RuleFor(el => el)
                     .Matches(new Regex(regEx))
                     .WithName(fieldName);
            return validator.Validate(fieldValue);
        }

        private static ValidationResult ValidateIfFieldIsNotEmpty(string fieldName, string fieldValue)
        {
            var validator = new CustomDynamicValidator<string>();

            validator.RuleFor(el => el).NotEmpty().WithName(fieldName);
            return validator.Validate(fieldValue);
        }
    }
}