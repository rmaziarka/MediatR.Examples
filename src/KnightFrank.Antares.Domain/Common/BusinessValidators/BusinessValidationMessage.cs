namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Globalization;

    public class BusinessValidationMessage
    {
        public readonly string ErrorCode;
        public readonly string Message;
        public const string EntityNotExistErrorMessgeKey = "Entity_Not_Exists";

        public BusinessValidationMessage(string errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        public static BusinessValidationMessage CreateEntityNotExistMessage(string entityType, Guid id)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en");

            string errorMessageTemplate = Properties.BusinessErrorMessages.ResourceManager.GetString("Entity_Not_Exists", culture) ?? "";

            string message = string.Format(errorMessageTemplate, entityType, id);
            return new BusinessValidationMessage(entityType + "_" + EntityNotExistErrorMessgeKey, message);
        }
    }
}