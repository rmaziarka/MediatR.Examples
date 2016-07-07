namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System;

    public static class TypeExtensions
    {
        public static bool IsSameOrSubclassOf(this Type possibleDerivedType, Type possibleBaseType)
        {
            return possibleDerivedType.IsSubclassOf(possibleBaseType) || possibleDerivedType == possibleBaseType;
        }
    }
}
