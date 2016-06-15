namespace KnightFrank.Antares.Search.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Search.Common.Queries;

    using Xunit;

    [CollectionDefinition("Common unit test collection")]
    public class BaseTestClassFixture : IDisposable
    {
        private static readonly bool isInitialized;

        private static readonly object sync = new object();

        static BaseTestClassFixture()
        {
            lock (sync)
            {
                if (isInitialized)
                {
                    return;
                }
                SetupAutoMapper();
                isInitialized = true;
            }
        }

        public void Dispose()
        {
        }

        private static void SetupAutoMapper()
        {
            Mapper.Initialize(
                cfg =>
                    {
                        IEnumerable<Type> profileTypes = from t in typeof(ISearchDescriptorQuery).Assembly.GetTypes()
                                                         where t.BaseType != null && t.BaseType == typeof(Profile)
                                                         select t;

                        foreach (Type profileType in profileTypes)
                        {
                            var profile = (Profile)Activator.CreateInstance(profileType);
                            cfg.AddProfile(profile);
                        }
                    });
        }
    }
}
