namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    public interface IField
    {
        void SetRequired();

        InnerField InnerField { get; }

        IField Copy();
    }
}