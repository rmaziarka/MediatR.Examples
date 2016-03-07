namespace KnightFrank.Antares.Api
{
    using System;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Domain;

    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                var profileTypes = from t in typeof(DomainModule).Assembly.GetTypes()
                                   where t.BaseType != null && t.BaseType == typeof(Profile)
                                   select t;

                foreach (var profileType in profileTypes)
                {
                    dynamic profile = Activator.CreateInstance(profileType);
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}