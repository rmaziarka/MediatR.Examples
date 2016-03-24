namespace KnightFrank.Antares.Domain
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    using MediatR;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;

    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(
                x =>
                x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces()
                 .Configure(y => y.WhenInjectedInto(typeof(ValidatorCommandHandler<,>))));

            this.Bind(typeof(IRequestHandler<,>)).To(typeof(ValidatorCommandHandler<,>));

            this.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));

            this.Bind(typeof(IReadGenericRepository<>)).To(typeof(ReadGenericRepository<>));

            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                           .ForEach(x =>
                           {
                               Type domainValidatorType = x.ValidatorType.GetInterfaces()
                                                      .Where(y => y.IsGenericType)
                                                      .Select(y => y.GetGenericTypeDefinition())
                                                      .FirstOrDefault(y => y == typeof(IDomainValidator<>));

                               this.Kernel.Bind(domainValidatorType ?? x.InterfaceType).To(x.ValidatorType);
                           });
        }
    }
}
