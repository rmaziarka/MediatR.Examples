using MediatR;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Domain.Logging
{
    public class LoggingNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(INotificationHandler<>))
                 .BindAllInterfaces());
        }
    }
}
