namespace KnightFrank.Antares.Domain.UnitTests
{
    using Ploeh.AutoFixture.Xunit2;

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values) : base(new AutoMoqDataAttribute(), values)
        {
            
        }
    }
}