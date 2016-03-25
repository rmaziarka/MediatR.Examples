/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Common.Models.Resources;
    import AddressFormResourceClass = Antares.Common.Models.Resources.IAddressFormResourceClass;
    import CountryResourceClass = Antares.Common.Models.Resources.ICountryResourceClass;

    export class DataAccessService {

        private rootUrl: string = "";

        constructor(private $resource: ng.resource.IResourceService, private appConfig: Antares.Common.Models.IAppConfig) {
        }

        private updateAction: ng.resource.IActionDescriptor = {
            method: 'PUT',
            isArray: false
        };

        private createOwnershipAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + '/api/properties/:propertyId/ownerships',
            method: 'POST',
            isArray: false,
            params: {
                propertyId: '@propertyId'
            }
        };

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(this.appConfig.rootUrl + '/api/contacts/:id');
        }

        getRequirementResource(): Resources.IBaseResourceClass<Resources.IRequirementResource> {
            return <Resources.IBaseResourceClass<Resources.IRequirementResource>>
                this.$resource(this.appConfig.rootUrl + '/api/requirements/:id');
        }

        getCountryResource(): CountryResourceClass {
            return <CountryResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/addressforms/countries?entityType=:entityTypeCode');
        }

        getAddressFormResource(): AddressFormResourceClass {
            return <AddressFormResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/addressForms/:id?entityType=:entityTypeCode&countryCode=:countryCode');
        }

        getPropertyResource(): Resources.IPropertyResourceClass {
            return <Resources.IPropertyResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/properties/:id', null, {
                    update: this.updateAction,
                    createOwnership: this.createOwnershipAction
                });
        }

        getEnumResource(): Resources.IBaseResourceClass<Resources.IEnumResource> {
            return <Resources.IBaseResourceClass<Resources.IEnumResource>>
                this.$resource(this.appConfig.rootUrl + '/api/enums/:code/items');
        }

        getActivityResource(): Resources.IBaseResourceClass<Resources.IActivityResource> {
            return <Resources.IBaseResourceClass<Resources.IActivityResource>>
                this.$resource(this.appConfig.rootUrl + '/api/activities/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}