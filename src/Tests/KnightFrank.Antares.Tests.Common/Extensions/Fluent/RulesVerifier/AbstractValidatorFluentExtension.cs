namespace KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Validators;

    using MediatR;

    public static class AbstractValidatorFluentExtension
    {
        public static void ShouldHaveRules<T, TProperty>(
            this AbstractValidator<T> validator,
            Expression<Func<T, TProperty>> expression,
            params IRuleVerifier[] validatorRuleVerifieres) where T : IRequest<Guid>
        {
            List<IPropertyValidator> validators = validator.Select(x => (PropertyRule)x).Where(r => r.Member == expression.GetMember()).SelectMany(x => x.Validators).ToList();

            validators.Should().HaveCount(validatorRuleVerifieres.Length);

            for (var i = 0; i < validatorRuleVerifieres.Length; i++)
            {
                validatorRuleVerifieres[i].Verify(validators[i]);
            }
        }

        public static void ShouldHaveRulesCount<T>(this AbstractValidator<T> validator, int rulesNumber)
        {
            validator.Count().ShouldBeEquivalentTo(rulesNumber);
        }
    }
}
