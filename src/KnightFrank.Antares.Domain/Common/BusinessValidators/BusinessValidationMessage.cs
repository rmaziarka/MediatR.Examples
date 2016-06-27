namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Globalization;

    public class BusinessValidationMessage
    {
        static readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en");
        public readonly ErrorMessage ErrorCode;
        public readonly string Message;

        public BusinessValidationMessage(ErrorMessage errorCode, string message) : this(errorCode)
        {
            this.Message = message;
        }

        public BusinessValidationMessage(ErrorMessage errorCode)
        {
            this.ErrorCode = errorCode;
            this.Message = GetMessage(errorCode);
        }

        public BusinessValidationMessage(ErrorMessage errorCode, params string[] messageParameters)
        {
            this.ErrorCode = errorCode;
            string messageTemplate = GetMessage(errorCode);
            this.Message = string.Format(messageTemplate, messageParameters);
        }

        public static BusinessValidationMessage CreateEntityNotExistMessage(string entityType, Guid id)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Entity_Not_Exists);
            string message = string.Format(errorMessageTemplate, entityType, id);
            return new BusinessValidationMessage(ErrorMessage.Entity_Not_Exists, message);
        }

        public static BusinessValidationMessage CreateOneOfEntitiesNotExistMessage(string entityType)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Entity_List_Item_Not_Exists);
            string message = string.Format(errorMessageTemplate, entityType);
            return new BusinessValidationMessage(ErrorMessage.Entity_List_Item_Not_Exists, message);
        }

        public static BusinessValidationMessage CreateEnumTypeItemNotExistMessage(string enumType, Guid id)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.EnumType_Item_Not_Exists);
            string message = string.Format(errorMessageTemplate, enumType, id);
            return new BusinessValidationMessage(ErrorMessage.EnumType_Item_Not_Exists, message);
        }

        public static BusinessValidationMessage CreateOnlyCommercialPropertyShouldHaveAreaBreakdownMessage()
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Only_Commercial_Property_Should_Have_AreaBreakdown);
            string message = string.Format(errorMessageTemplate);
            return new BusinessValidationMessage(ErrorMessage.Only_Commercial_Property_Should_Have_AreaBreakdown, message);
        }

        public static BusinessValidationMessage CreatePropertyShouldBeEmptyMessage(string propertyName)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Property_Should_Be_Empty);
            string message = string.Format(errorMessageTemplate, propertyName);
            return new BusinessValidationMessage(ErrorMessage.Property_Should_Be_Empty, message);
        }

        // todo: if there will be many methods creating object with the same paramters think about refactoring
        public static BusinessValidationMessage CreatePropertyShouldNotBeEmptyMessage(string propertyName)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Property_Should_Not_Be_Empty);
            string message = string.Format(errorMessageTemplate, propertyName);
            return new BusinessValidationMessage(ErrorMessage.Property_Should_Not_Be_Empty, message);
        }

        public static BusinessValidationMessage CreatePropertyFormatIsInvalidMessage(string propertyName)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Property_Format_Is_Invalid);
            string message = string.Format(errorMessageTemplate, propertyName);
            return new BusinessValidationMessage(ErrorMessage.Property_Format_Is_Invalid, message);
        }

        public static BusinessValidationMessage CreateInconsistentDynamicConfigurationMessage(string configurationName)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Inconsistent_Dynamic_Configuration);
            string message = string.Format(errorMessageTemplate, configurationName);
            return new BusinessValidationMessage(ErrorMessage.Inconsistent_Dynamic_Configuration, message);
        }

        public static BusinessValidationMessage OfferDateLessOrEqualToCreateDateMessage(DateTime createDate)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.OfferDateLessOrEqualToCreateDate);
            string message = string.Format(errorMessageTemplate, createDate.Date.ToShortDateString());
            return new BusinessValidationMessage(ErrorMessage.OfferDateLessOrEqualToCreateDate, message);
        }

        public static BusinessValidationMessage ExchangeDateGreaterOrEqualToCreateDateMessage(DateTime createDate)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.ExchangeDateGreaterOrEqualToCreateDate);
            string message = string.Format(errorMessageTemplate, createDate.Date.ToShortDateString());
            return new BusinessValidationMessage(ErrorMessage.ExchangeDateGreaterOrEqualToCreateDate, message);
        }

        public static BusinessValidationMessage CompletionDateGreaterOrEqualToCreateDateMessage(DateTime createDate)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.CompletionDateGreaterOrEqualToCreateDate);
            string message = string.Format(errorMessageTemplate, createDate.Date.ToShortDateString());
            return new BusinessValidationMessage(ErrorMessage.CompletionDateGreaterOrEqualToCreateDate, message);
        }

        public static BusinessValidationMessage MortgageSurveyDateGreaterOrEqualToCreateDateMessage(DateTime createDate)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.MortgageSurveyDateGreaterOrEqualToCreateDate);
            string message = string.Format(errorMessageTemplate, createDate.Date.ToShortDateString());
            return new BusinessValidationMessage(ErrorMessage.MortgageSurveyDateGreaterOrEqualToCreateDate, message);
        }

        public static BusinessValidationMessage AdditionalSurveyDateGreaterOrEqualToCreateDateMessage(DateTime createDate)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.AdditionalSurveyDateGreaterOrEqualToCreateDate);
            string message = string.Format(errorMessageTemplate, createDate.Date.ToShortDateString());
            return new BusinessValidationMessage(ErrorMessage.AdditionalSurveyDateGreaterOrEqualToCreateDate, message);
        }

        private static string GetMessage(ErrorMessage errorCode)
        {
            return Properties.BusinessErrorMessages.ResourceManager.GetString(GetErrorName(errorCode), culture) ?? "";
        }

        private static string GetErrorName(ErrorMessage errorCode)
        {
            return Enum.GetName(typeof(ErrorMessage), errorCode);
        }
    }
}