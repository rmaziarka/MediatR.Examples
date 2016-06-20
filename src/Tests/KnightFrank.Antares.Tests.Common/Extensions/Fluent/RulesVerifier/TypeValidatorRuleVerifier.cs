namespace KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier
{
    using FluentAssertions;

    using FluentValidation.Validators;

    public class TypeValidatorRuleVerifier<T> : IRuleVerifier where T : IPropertyValidator
    {
        public virtual void Verify<TValidator>(TValidator validator)
        {
            validator.Should().BeOfType<T>();
        }
    }
}
