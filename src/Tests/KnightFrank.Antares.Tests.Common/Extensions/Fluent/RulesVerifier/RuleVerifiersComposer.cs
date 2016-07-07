namespace KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier
{
    using System.Collections.Generic;

    using FluentValidation.Validators;

    public class RuleVerifiersComposer
    {
        private readonly List<IRuleVerifier> ruleVerifiers;

        private RuleVerifiersComposer()
        {
            this.ruleVerifiers = new List<IRuleVerifier>();
        }

        public static RuleVerifiersComposer Build()
        {
            return new RuleVerifiersComposer();
        }

        public RuleVerifiersComposer AddPropertyValidatorVerifier<T>() where T : IPropertyValidator
        {
            this.ruleVerifiers.Add(new TypeValidatorRuleVerifier<T>());
            return this;
        }

        public RuleVerifiersComposer AddPropertyValidatorVerifier<T>(object valueToCompare) where T : IComparisonValidator
        {
            this.ruleVerifiers.Add(new ComparisonValidatorRuleVerifier<T>(valueToCompare));
            return this;
        }

        public RuleVerifiersComposer AddPropertyValidatorVerifier<T>(int min, int max) where T : ILengthValidator
        {
            this.ruleVerifiers.Add(new LengthValidatorRuleVerifier<T>(min, max));
            return this;
        }

        public RuleVerifiersComposer AddChildValidatorVerifier<T>()
        {
            this.ruleVerifiers.Add(new ChildValidatorRuleVerifier<T>());
            return this;
        }

        public RuleVerifiersComposer AddChildCollectionValidatorVerifier<T>()
        {
            this.ruleVerifiers.Add(new ChildCollectionValidatorRuleVerifier<T>());
            return this;
        }

        public RuleVerifiersComposer AddVerifier(IRuleVerifier ruleVerifier)
        {
            this.ruleVerifiers.Add(ruleVerifier);
            return this;
        }

        public RuleVerifiersComposer AddCustomVerifier()
        {
            this.ruleVerifiers.Add(new CustomRuleVerifier());
            return this;
        }

        public IRuleVerifier[] Create()
        {
            return this.ruleVerifiers.ToArray();
        }
    }
}
