using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Command;
using FluentValidation;
using MediatR;

namespace Common.Handler
{
    public abstract class ValidatorHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public abstract TResponse HandleRequest(TRequest request);

        private IValidator CreateValidatorForCommand(TRequest request)
        {
            var vt = typeof(AbstractValidator<>);
            var et = request.GetType();
            var evt = vt.MakeGenericType(et);

            var validatorType = Assembly.GetAssembly(et).GetTypes().FirstOrDefault(t => t.IsSubclassOf(evt)); ;
            if (validatorType == null)
                return null;

            var validatorInstance = (IValidator)Activator.CreateInstance(validatorType);
            return validatorInstance;
        }

        public TResponse Handle(TRequest request)
        {
            var validator = CreateValidatorForCommand(request);
            if(validator == null)
                return HandleRequest(request);

            var context = new ValidationContext(request);
            var failures = validator
                .Validate(context)
                .Errors
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return HandleRequest(request);
        }
    }
}
