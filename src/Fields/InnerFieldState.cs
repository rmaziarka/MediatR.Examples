using System;
using System.Collections.Generic;

namespace Fields
{
    using System.Linq.Expressions;

    using FluentValidation;

    public class InnerFieldState
    {
        public bool IsReadonly { get; set; }
        public bool IsHidden { get; set; }
        public IList<IValidator> Validators { get; set; }
        public Func<object, object> Compiled { get; set; }
        public LambdaExpression Expression { get; set; }
        public Type ContainerType { get; set; }
        public Type PropertyType { get; set; }
        public string Name { get; set; }
    }
}
