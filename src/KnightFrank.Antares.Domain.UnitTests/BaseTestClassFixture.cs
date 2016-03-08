namespace KnightFrank.Antares.Domain.UnitTests
{
    using System;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Xunit;

    [CollectionDefinition("Common unit test collection")]
    public class BaseTestClassFixture : IDisposable
    {
        public BaseTestClassFixture()
        {
            Mapper.Initialize(cfg =>
            {
                var profileTypes = from t in typeof(CreateRequirementCommand).Assembly.GetTypes()
                                   where t.BaseType != null && t.BaseType == typeof(Profile)
                                   select t;

                foreach (var profileType in profileTypes)
                {
                    var profile = (Profile)Activator.CreateInstance(profileType);
                    cfg.AddProfile(profile);
                }
            });
        }

        public void Dispose()
        {
        }
    }
}
