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

        public static BusinessValidationMessage CreateEntityNotExistMessage(string entityType, Guid id)
        {
            string errorMessageTemplate = GetMessage(ErrorMessage.Entity_Not_Exists);
            string message = string.Format(errorMessageTemplate, entityType, id);
            return new BusinessValidationMessage(ErrorMessage.Entity_Not_Exists, message);
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