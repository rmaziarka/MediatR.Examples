namespace KnightFrank.Antares.Api.Core
{
    using System.Web;

    public static class NinjectExtentions
    {
        public static Ninject.Syntax.IBindingNamedWithOrOnSyntax<T> InCustomHttpContextRequestScope<T>(this Ninject.Syntax.IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(ctx => HttpContext.Current.Handler == null ? null : HttpContext.Current.Request);
        }
    }
}