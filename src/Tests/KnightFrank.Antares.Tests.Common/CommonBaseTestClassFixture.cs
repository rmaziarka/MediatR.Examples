namespace KnightFrank.Antares.Tests.Common
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Xunit;

    [CollectionDefinition("Common unit test collection")]
    public abstract class CommonBaseTestClassFixture<T> : IDisposable
    {
        static CommonBaseTestClassFixture()
        {
            SetupAutoMapper();
        }

        private static void SetupAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                var profileTypes = from t in typeof(T).Assembly.GetTypes()
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
