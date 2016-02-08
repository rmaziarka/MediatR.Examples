using MediatR;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Domain.Order
{
    public class OrderNinjectModule : NinjectModule
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
