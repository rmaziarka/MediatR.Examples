namespace KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier
{
    public interface IRuleVerifier
    {
        void Verify<T>(T validator);
    }
}
