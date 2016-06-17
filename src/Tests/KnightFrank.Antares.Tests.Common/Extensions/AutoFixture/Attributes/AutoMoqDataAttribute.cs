namespace KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes
{
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Builders;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            this.Fixture.Behaviors.Clear();

            this.Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.Fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}