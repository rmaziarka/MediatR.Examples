using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fields.Validators
{
    using FluentValidation;

    public class DecimalValidator:InlineValidator<decimal?>
    {
        public DecimalValidator()
        {
            
        }
        public DecimalValidator(params Action<InlineValidator<decimal?>>[] actions)
        {
            foreach (var action in actions)
            {
                action(this);
            }

        }
    }

    public class EntityValidator<T> : InlineValidator<T>
    {
        public EntityValidator()
        {
            
        }

        public EntityValidator(params Action<InlineValidator<T>>[] actions)
        {
            foreach (var action in actions)
            {
                action(this);
            }
        }
    }
}
