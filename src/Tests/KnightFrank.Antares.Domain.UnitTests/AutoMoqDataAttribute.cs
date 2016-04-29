namespace KnightFrank.Antares.Domain.UnitTests
{
    using KnightFrank.Antares.Domain.UnitTests.Autofixture;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            this.Fixture.Behaviors.Clear();

            this.Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.Fixture.Customize(new IgnoreVirtualMembersCustomisation());
        }
    }
}