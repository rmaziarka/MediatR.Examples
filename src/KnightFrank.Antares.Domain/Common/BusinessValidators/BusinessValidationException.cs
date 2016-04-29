namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    public class BusinessValidationException : Exception
    {
        private readonly BusinessValidationMessage businessMessage;

        public override string Message => this.businessMessage.Message;

        public ErrorMessage ErrorCode => this.businessMessage.ErrorCode;

        public BusinessValidationException(BusinessValidationMessage businessMessage)
        {
            this.businessMessage = businessMessage;
        }

        public BusinessValidationException(ErrorMessage errorCode)
        {
            this.businessMessage = new BusinessValidationMessage(errorCode);
        }
    }
}