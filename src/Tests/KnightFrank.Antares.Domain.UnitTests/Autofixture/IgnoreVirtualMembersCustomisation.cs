namespace KnightFrank.Antares.Domain.UnitTests.Autofixture
{
    using Ploeh.AutoFixture;

    public class IgnoreVirtualMembersCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}