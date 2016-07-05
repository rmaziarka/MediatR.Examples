namespace KnightFrank.Antares.Domain.UnitTests.Common.Extension
{
    using System;

    public static class ArrayExtension
    {
        public static T[] Concat<T>(this T[] arrayA, T[] arrayB)
        {
            if (arrayA == null)
            {
                throw new ArgumentNullException("arrayA");
            }
            if (arrayB == null)
            {
                throw new ArgumentNullException("arrayB");
            }
            int oldLen = arrayA.Length;
            Array.Resize(ref arrayA, arrayA.Length + arrayB.Length);
            Array.Copy(arrayB, 0, arrayA, oldLen, arrayB.Length);
            return arrayA;
        }
    }
}
