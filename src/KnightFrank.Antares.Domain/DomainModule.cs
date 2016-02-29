using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Conventions;
namespace KnightFrank.Antares.Domain
{
    using MediatR;

    using Ninject.Modules;

    public class DomainModule: NinjectModule
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
