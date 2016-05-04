namespace KnightFrank.Antares.Domain.UnitTests.FixtureExtension
{
    using KnightFrank.Antares.Domain.UnitTests.Autofixture;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public static class FixtureExtensions
    {
        public static Fixture Customize(this Fixture fixture)
        {
            fixture.Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();

            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new IgnoreVirtualMembersCustomisation());

            return fixture;
        }
    }
}