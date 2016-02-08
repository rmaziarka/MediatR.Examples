using MediatR;
using Ninject.Modules;

using Ninject.Extensions.Conventions;
namespace Domain.Statistics
{
    public class StatisticsNinjectModule : NinjectModule
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
