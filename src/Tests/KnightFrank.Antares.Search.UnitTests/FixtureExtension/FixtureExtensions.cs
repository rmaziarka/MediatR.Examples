namespace KnightFrank.Antares.Search.UnitTests.FixtureExtension
{
    using KnightFrank.Antares.Search.UnitTests.Autofixture;

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
            fixture.Customizations.Add(new IgnoreVirtualMembers());

            return fixture;
        }
    }
}
