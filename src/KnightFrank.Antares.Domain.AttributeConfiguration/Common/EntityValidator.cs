namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;

    using FluentValidation;

    public class EntityValidator<T> : InlineValidator<T>
    {
        public EntityValidator()
        {
            
        }

        public EntityValidator(params Action<InlineValidator<T>>[] actions)
        {
            foreach (Action<InlineValidator<T>> action in actions)
            {
                action(this);
            }
        }
    }
}
