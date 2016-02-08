using MediatR;
using Ninject.Modules;

using Ninject.Extensions.Conventions;
namespace Domain.Account
{
    public class AccountNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces());
        }
    }
}
