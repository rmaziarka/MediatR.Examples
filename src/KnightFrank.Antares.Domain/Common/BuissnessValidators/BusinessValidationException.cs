namespace KnightFrank.Antares.Domain.Common.BuissnessValidators
{
    using System;

    using KnightFrank.Antares.Domain.Validators;

    public class BusinessValidationException : Exception
    {
        private readonly BusinessValidationMessage buissnessMessage;
        public override string Message => this.buissnessMessage.Message;

        public string ErrorCode => this.buissnessMessage.ErrorCode;

        public BusinessValidationException(BusinessValidationMessage buissnessMessage)
        {
            this.buissnessMessage = buissnessMessage;
        }
    }
}