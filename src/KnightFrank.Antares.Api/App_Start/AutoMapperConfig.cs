namespace KnightFrank.Antares.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Domain;
    using KnightFrank.Antares.Search;

    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
                {
                    var assemblyTypes = typeof(DomainModule).Assembly.GetTypes().Union(typeof(SearchModule).Assembly.GetTypes());
                    IEnumerable<Type> profileTypes = from t in assemblyTypes
                                                     where t.BaseType != null && t.BaseType == typeof(Profile)
                                                     select t;

                foreach (var profileType in profileTypes)
                {
                    var profile = (Profile)Activator.CreateInstance(profileType);
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}