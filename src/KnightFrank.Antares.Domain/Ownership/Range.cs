using System;

namespace KnightFrank.Antares.Domain.Ownership
{
    public class Range<T> where T : IComparable
    {
        public Range(T min, T max)
        {
            this.Min = min;
            this.Max = max;
        }

        public bool IsOverlapped(Range<T> other)
        {
            return this.Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(this.Max) < 0;
        }

        public T Min { get; }
        public T Max { get; }
    }
}
