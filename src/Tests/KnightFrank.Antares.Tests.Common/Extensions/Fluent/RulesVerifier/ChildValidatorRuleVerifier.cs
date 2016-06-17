namespace KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier
{
    using FluentAssertions;

    using FluentValidation.Validators;

    public class ChildValidatorRuleVerifier<T> : IRuleVerifier
    {
        public void Verify<TChildValidatorAdaptor>(TChildValidatorAdaptor validator)
        {
            validator.Should().BeOfType<ChildValidatorAdaptor>();

            // ReSharper disable once PossibleNullReferenceException
            (validator as ChildValidatorAdaptor).ValidatorType.Should().Be(typeof(T));
        }
    }
}
