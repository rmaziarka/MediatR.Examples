namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public class FieldActions<TEntity, TProperty> : Dictionary<Expression<Func<TEntity, TProperty>>, Action<IList<Field<TEntity, TProperty>>>>
    {
    }
}