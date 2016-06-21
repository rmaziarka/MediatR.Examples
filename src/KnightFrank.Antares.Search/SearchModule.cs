namespace KnightFrank.Antares.Search
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Search.Common.SearchDescriptors;
    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.Queries;
    using KnightFrank.Antares.Search.Property.QueryHandlers;

    using Nest;

    using Ninject.Modules;

    public class SearchModule : NinjectModule
    {
        private static readonly string ApiSettingsElasticSearchIndex = "Api.Settings.ElasticSearchIndex";

        private static readonly string ApiSettingsElasticSearchUrl = "Api.Settings.ElasticSearchUrl";

        public override void Load()
        {
            this.Bind<IElasticClient>()
                .To<ElasticClient>()
                .WithConstructorArgument("connectionSettings", GetElasticSearchConnectionSettings());

            this.Bind<ISearchDescriptor<Contact, ContactsSearchDescriptorQuery>>().To(typeof(ContactsSearchDescriptor));
            this.Bind<ISearchDescriptor<Models.Property, PropertiesPageableQuery>>().To(typeof(PropertiesSearchDescriptor));

            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                           .ForEach(
                               assemblyScanResult =>
                                   {
                                       this.Kernel.Bind(assemblyScanResult.InterfaceType).To(assemblyScanResult.ValidatorType);
                                   });
        }

        private static ConnectionSettings GetElasticSearchConnectionSettings()
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            var uri = new Uri(config[ApiSettingsElasticSearchUrl]);
            string defaultIndex = config[ApiSettingsElasticSearchIndex];

            return
                new ConnectionSettings(uri).DefaultIndex(defaultIndex)
                                           .PrettyJson(true)
                                           .DisableDirectStreaming()
                                           .DefaultFieldNameInferrer(x => x);
        }
    }
}
