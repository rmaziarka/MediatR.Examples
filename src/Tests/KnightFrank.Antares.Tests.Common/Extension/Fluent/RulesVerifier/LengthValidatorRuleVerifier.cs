namespace KnightFrank.Antares.Tests.Common.Extension.Fluent.RulesVerifier
{
    using FluentAssertions;

    using FluentValidation.Validators;

    public class LengthValidatorRuleVerifier<T> : TypeValidatorRuleVerifier<T> where T : ILengthValidator
    {
        private readonly int min;

        private readonly int max;

        public LengthValidatorRuleVerifier(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override void Verify<TValidator>(TValidator validator)
        {
            base.Verify(validator);
            var lengthValidator = (ILengthValidator)validator;
            lengthValidator.Min.ShouldBeEquivalentTo(this.min);
            lengthValidator.Max.ShouldBeEquivalentTo(this.max);
        }
    }
}