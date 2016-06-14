namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class InnerDictionaryField : InnerField
    {
        public readonly string DictionaryCode;
        public IList<string> AllowedCodes;

        public InnerDictionaryField(MemberInfo member, Func<object, object> compiled, LambdaExpression expression, Type containerType, Type propertyType, string dictionaryCode) 
            : base(member, compiled, expression, containerType, propertyType)
        {
            this.DictionaryCode = dictionaryCode;
        }
    }
}