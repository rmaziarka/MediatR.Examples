namespace KnightFrank.Antares.Domain.UnitTests
{
    using System;
    using System.Reflection;

    using Ploeh.AutoFixture.Kernel;

    public class IgnoreVirtualMembers : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen(request);
            }

            if (pi.GetGetMethod().IsVirtual)
            {
                return null;
            }

            return new NoSpecimen(request);
        }
    }
}