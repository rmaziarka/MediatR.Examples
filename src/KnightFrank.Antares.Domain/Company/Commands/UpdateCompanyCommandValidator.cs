namespace KnightFrank.Antares.Domain.Company.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Domain.Company.CustomValidators;

    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator(ICompanyCommandCustomValidator companyCommandCustomValidator)
        {
            this.RuleFor(p => p.Name).NotEmpty().Length(1, 128);
            this.RuleFor(p => p.Contacts).NotEmpty();
            this.RuleFor(p => p.WebsiteUrl).Length(0, 2500);
            this.RuleFor(p => p.Description).Length(0, 4000);
            this.RuleFor(p => p.ClientCarePageUrl).Length(0, 2500);
            this.RuleFor(p => p.ClientCareStatusId).Must(companyCommandCustomValidator.IsClientCareEnumValid);
            this.RuleFor(p => p.CompanyTypeId).Must(companyCommandCustomValidator.IsCompanyTypeEnumValid);
            this.RuleFor(p => p.CompanyCategoryId).Must(companyCommandCustomValidator.IsCompanyCategoryEnumValid);
            this.RuleFor(p => p.RelationshipManagerId).Must(companyCommandCustomValidator.IsRelationshipManagerValid);
        }
    }
}