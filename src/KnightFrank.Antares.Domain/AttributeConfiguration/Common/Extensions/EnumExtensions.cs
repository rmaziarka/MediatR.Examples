namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ParseEnum<T>(string value) where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public static bool TryParseEnum<T>(string value, out T result) where T : struct
        {
            return System.Enum.TryParse(value, out result);
        }
    }
}
