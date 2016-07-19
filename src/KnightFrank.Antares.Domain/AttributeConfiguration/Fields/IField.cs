namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    public interface IField
    {
        InnerField InnerField { get; }

        IField Copy();
    }
}