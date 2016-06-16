namespace KnightFrank.Antares.Tests.Common.Extension.Fluent.RulesVerifier
{
    using FluentAssertions;

    using FluentValidation.Validators;

    public class ComparisonValidatorRuleVerifier<T> : TypeValidatorRuleVerifier<T> where T : IComparisonValidator
    {
        private readonly object valueToCompare;

        public ComparisonValidatorRuleVerifier(object valueToCompare)
        {
            this.valueToCompare = valueToCompare;
        }

        public override void Verify<TValidator>(TValidator validator)
        {
            base.Verify(validator);
            ((IComparisonValidator)validator).ValueToCompare.ShouldBeEquivalentTo(this.valueToCompare);
        }
    }
}
