namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentValidation;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public class InnerFieldState
    {
        public bool Required { get; set; }
        public bool Readonly { get; set; }
        public bool Hidden { get; set; }
        public IList<IValidator> Validators { get; set; }
        public Func<object, object> Compiled { get; set; }
        public LambdaExpression Expression { get; set; }
        public Type ContainerType { get; set; }
        public Type PropertyType { get; set; }
        public string Name { get; set; }
        public ControlCode ControlCode { get; set; }
        public string DictionaryCode { get; set; }
        public IList<string> AllowedCodes { get; set; } 
    }
}
