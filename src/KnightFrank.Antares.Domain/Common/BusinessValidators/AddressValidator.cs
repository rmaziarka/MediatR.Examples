namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Commands;

    public class AddressValidator : IAddressValidator
    {
        private readonly IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository;
        private readonly IGenericRepository<AddressForm> addressFormRepository;
        private readonly IEntityValidator entityValidator;

        public AddressValidator(IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository, IGenericRepository<AddressForm> addressFormRepository, IEntityValidator entityValidator)
        {
            this.addressFieldDefinitionRepository = addressFieldDefinitionRepository;
            this.addressFormRepository = addressFormRepository;
            this.entityValidator = entityValidator;
        }
        public void Validate(CreateOrUpdateAddress address)
        {
            AddressForm addressForm = this.addressFormRepository.GetById(address.AddressFormId);
            this.entityValidator.EntityExists(addressForm, address.AddressFormId);

            ValidateAgainstCountryConsistency(address.CountryId, addressForm.CountryId);

            Dictionary<string, string> addressFields = ConvertQueryToAddressFields(address);

            List<AddressFieldDefinition> addressFieldDefinitions =
                this.addressFieldDefinitionRepository.GetWithInclude(
                    afd => afd.AddressFormId == address.AddressFormId,
                    afd => afd.AddressForm,
                    adf => adf.AddressField).ToList();


            ValidateIfQueryHasMoreDataThenConfigured(addressFields, addressFieldDefinitions);
            ValidateQueryAgainstConfiguration(addressFieldDefinitions, addressFields);
        }

        private static Dictionary<string, string> ConvertQueryToAddressFields(CreateOrUpdateAddress address)
        {
            List<PropertyInfo> propertyInfos = address.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();

            var addressFields = new Dictionary<string, string>();

            propertyInfos.ForEach(pi => { addressFields.Add(pi.Name, pi.GetValue(address) as string); });

            return addressFields;
        }

        private static void ValidateAgainstCountryConsistency(Guid addressCountryId, Guid addressFormCountryId)
        {
            if (addressCountryId != addressFormCountryId)
            {
                throw new BusinessValidationException(ErrorMessage.Inconsistent_Address_Country_Id);
            }
        }

        private static void ValidateIfQueryHasMoreDataThenConfigured(Dictionary<string, string> addressFields, List<AddressFieldDefinition> addressFieldDefinitions)
        {
            var fieldsWithDefinition = from addressField in addressFields.Where(x => !string.IsNullOrWhiteSpace(x.Value))
                                       join addressFieldDefinition in addressFieldDefinitions on addressField.Key equals
                                           addressFieldDefinition.AddressField.Name into result
                                       from r in result.DefaultIfEmpty()
                                       select new { AddressFieldDefinition = r, Field = new { addressField.Value, addressField.Key } };

            fieldsWithDefinition
                .Where(x => x.AddressFieldDefinition == null)
                .ToList().ForEach(x =>
                {
                    throw new BusinessValidationException(BusinessValidationMessage.CreatePropertyShouldBeEmptyMessage(x.Field.Key));
                });
        }

        private static void ValidateQueryAgainstConfiguration(List<AddressFieldDefinition> addressFieldDefinitions, Dictionary<string, string> addressFields)
        {
            var joinResult =
                addressFieldDefinitions.Join(addressFields, p => p.AddressField.Name, pi => pi.Key,
                    (p, pi) => new { AddressFieldDefinition = p, pi.Value }).ToList();

            bool isInconsistencyBetweenQueryAndDb = joinResult.Count != addressFieldDefinitions.Count;

            if (isInconsistencyBetweenQueryAndDb)
            {
                throw new BusinessValidationException(
                    BusinessValidationMessage.CreateInconsistentDynamicConfigurationMessage("Address"));
            }

            joinResult
                .Where(f => f.AddressFieldDefinition.Required)
                .ToList().ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.Value))
                    {
                        throw new BusinessValidationException(
                            BusinessValidationMessage.CreatePropertyShouldNotBeEmptyMessage(
                                f.AddressFieldDefinition.AddressField.Name));
                    }
                });

            joinResult
                .Where(f => !string.IsNullOrWhiteSpace(f.AddressFieldDefinition.RegEx))
                .ToList()
                .ForEach(
                    f =>
                        ValidateIfFieldMatchesRegEx(f.AddressFieldDefinition.AddressField.Name, f.Value,
                            f.AddressFieldDefinition.RegEx));
        }

        private static void ValidateIfFieldMatchesRegEx(string fieldName, string fieldValue, string regEx)
        {
            var regex = new Regex(regEx, RegexOptions.IgnoreCase);
            Match match = regex.Match(fieldValue ?? string.Empty);

            if (!match.Success)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreatePropertyFormatIsInvalidMessage(fieldName));
            }
        }
    }
}