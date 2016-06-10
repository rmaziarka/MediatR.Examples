using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fields.Validators
{
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
