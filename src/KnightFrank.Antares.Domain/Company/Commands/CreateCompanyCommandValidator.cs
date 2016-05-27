namespace KnightFrank.Antares.Domain.Company.Commands
{
    using System;

    using FluentValidation;

 using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public CreateCompanyCommandValidator(IEnumTypeItemValidator enumTypeItemValidator)
        {
            this.enumTypeItemValidator = enumTypeItemValidator;

            this.RuleFor(p => p.Name).NotEmpty().Length(1, 128);
            this.RuleFor(p => p.ContactIds).NotEmpty();
            this.RuleFor(p => p.WebsiteUrl).Length(1, 1000);
            this.RuleFor(p => p.ClientCarePageUrl).Length(1, 1000);
            this.RuleFor(p => p.ClientCareStatusId).Must(this.IsValidEnum);

        }

        private bool IsValidEnum(Guid? enumItemId)
        {
            if (enumItemId != null)
            {
                this.enumTypeItemValidator.ItemExists(EnumType.ClientCareStatus,(Guid) enumItemId);
            }
            return true;
        }
    }
}