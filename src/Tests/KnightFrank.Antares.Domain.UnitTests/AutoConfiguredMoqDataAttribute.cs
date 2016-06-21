namespace KnightFrank.Antares.Domain.UnitTests
{
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Builders;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    public class AutoConfiguredMoqDataAttribute : AutoDataAttribute
    {
        public AutoConfiguredMoqDataAttribute() : base(new Fixture().Customize(new AutoConfiguredMoqCustomization()))
        {
            this.Fixture.Behaviors.Clear();

            this.Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.Fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}