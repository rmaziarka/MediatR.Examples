/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Common.Models.Resources;
    import AddressFormResourceClass = Antares.Common.Models.Resources.IAddressFormResourceClass;
    import CountryResourceClass = Antares.Common.Models.Resources.ICountryResourceClass;

    export class DataAccessService {

        private rootUrl: string = "";

        constructor(private $resource: ng.resource.IResourceService, private appConfig: Antares.Common.Models.IAppConfig) {
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(this.appConfig.rootUrl + '/api/contact/:id');
        }

        getRequirementResource(): Resources.IBaseResourceClass<Resources.IRequirementResource> {
            return <Resources.IBaseResourceClass<Resources.IRequirementResource>>
                this.$resource(this.appConfig.rootUrl + '/api/requirement/:id');
        }

        getCountryResource(): CountryResourceClass {
            return <CountryResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/addressform/countries?entityType=:entityTypeCode');
        }

        getAddressFormResource(): AddressFormResourceClass {
            return <AddressFormResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/addressForm?entityType=:entityTypeCode&countryCode=:countryCode');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}