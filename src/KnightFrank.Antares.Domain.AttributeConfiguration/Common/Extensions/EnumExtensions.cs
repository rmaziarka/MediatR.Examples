namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
