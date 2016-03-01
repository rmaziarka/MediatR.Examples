namespace KnightFrank.Antares.Domain
{
    using KnightFrank.Antares.Dal.Repository;

    using MediatR;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;

    public class DomainModule: NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces());

            this.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
        }
    }
}
