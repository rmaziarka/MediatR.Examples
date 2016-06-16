namespace KnightFrank.Antares.Tests.Common.Extension.Fluent.RulesVerifier
{
    using FluentAssertions;

    using FluentValidation.Validators;

    public class ChildCollectionValidatorRuleVerifier<T> : IRuleVerifier
    {
        public void Verify<TChildValidatorAdaptor>(TChildValidatorAdaptor validator)
        {
            validator.Should().BeOfType<ChildCollectionValidatorAdaptor>();

            // ReSharper disable once PossibleNullReferenceException
            (validator as ChildCollectionValidatorAdaptor).ChildValidatorType.Should().Be(typeof(T));
        }
    }
}
