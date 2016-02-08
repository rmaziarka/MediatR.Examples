using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Domain.Catalog
{
    public class CatalogNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
            {
                x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindToSelf();
            });
        }
    }
}
