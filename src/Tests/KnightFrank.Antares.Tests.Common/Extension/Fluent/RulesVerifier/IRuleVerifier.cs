namespace KnightFrank.Antares.Tests.Common.Extension.Fluent.RulesVerifier
{
    public interface IRuleVerifier
    {
        void Verify<T>(T validator);
    }
}
