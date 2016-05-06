/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Common.Models.Resources;

    export class DataAccessService {
        private rootUrl: string = "";

        constructor(private $resource: ng.resource.IResourceService, private appConfig: Common.Models.IAppConfig) {
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

        private createViewingAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + '/api/viewings/',
            method: 'POST',
            isArray: false,
            params: {}
        };

        private getPropertyTypesAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + '/api/properties/types?countryCode=:countryCode&divisionCode=:divisionCode&localeCode=:localeCode',
            method: 'GET',
            isArray: false,
            params: {
                countryCode: '@countryCode',
                divisionCode: '@divisionCode',
                localeCode: '@localeCode'
            }
        };

        private getActivityTypesAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + '/api/activities/types?countryCode=:countryCode&propertyTypeId=:propertyTypeId',
            method: 'GET',
            isArray: true,
            params: {
                countryCode: '@countryCode',
                propertyTypeId: '@propertyTypeId'
            }
        };

        private getAttributesAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + '/api/properties/attributes?countryId=:countryId&propertyTypeId=:propertyTypeId',
            method: 'GET',
            isArray: false,
            params: {
                countryId: '@countryId',
                propertyTypeId: '@propertyTypeId'
            }
        };

        private getUrlAction: ng.resource.IActionDescriptor = {
            url: this.appConfig.rootUrl + 'api/services/attachment/upload/activity?activityDocumentType=:activityDocumentType&parameters.localeIsoCode=:localeIsoCode&parameters.externalDocumentId=:externalDocumentId&parameters.entityReferenceId=3&parameters.filename=:filename',
            method: 'GET',
            isArray: false,
            params: {
                activityDocumentType: '@activityDocumentType',
                localeIsoCode: '@localeIsoCode',
                externalDocumentId: '@externalDocumentId',
                filename: '@filename'
            }
        };

        getCompanyResource(): Resources.IBaseResourceClass<Resources.ICompanyResource> {
            return <Resources.IBaseResourceClass<Resources.ICompanyResource>>
                this.$resource(this.appConfig.rootUrl + '/api/companies/:id');
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(this.appConfig.rootUrl + '/api/contacts/:id');
        }

        getRequirementResource(): Resources.IRequirementResourceClass {
            return <Resources.IRequirementResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/requirements/:id');
        }

        getViewingResource(): Resources.IViewingResourceClass {
            return <Resources.IViewingResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/viewing/:id', null, {
                    createViewing: this.createViewingAction
                });
        }

        getCountryResource(): Resources.ICountryResourceClass {
            return <Resources.ICountryResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/resources/countries/addressform?entityTypeItemCode=:entityTypeCode');
        }

        getAddressFormResource(): Resources.IAddressFormResourceClass {
            return <Resources.IAddressFormResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/addressForms/:id?entityType=:entityTypeCode&countryCode=:countryCode');
        }

        getPropertyResource(): Resources.IPropertyResourceClass {
            return <Resources.IPropertyResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/properties/:id', null, {
                    update: this.updateAction,
                    createOwnership: this.createOwnershipAction,
                    getPropertyTypes: this.getPropertyTypesAction,
                    getAttributes: this.getAttributesAction
                });
        }

        getEnumsResource(): Resources.IBaseResourceClass<ng.resource.IResource<Antares.Common.Models.Dto.IEnumDictionary>> {
            return <Resources.IBaseResourceClass<ng.resource.IResource<Antares.Common.Models.Dto.IEnumDictionary>>>
                this.$resource(this.appConfig.rootUrl + '/api/enums');
        }

        getEnumTranslationResource(): Resources.ITranslationResourceClass<any> {
            return <Resources.ITranslationResourceClass<any>>
                this.$resource(this.appConfig.rootUrl + '/api/translations/enums/:isoCode');
        }

        getResourceTranslationResource(): Resources.ITranslationResourceClass<any> {
            return <Resources.ITranslationResourceClass<any>>
                this.$resource(this.appConfig.rootUrl + '/api/translations/resources/:isoCode');
        }

        getStaticTranslationResource(): Resources.ITranslationResourceClass<any> {
            return <Resources.ITranslationResourceClass<any>>
                this.$resource('/translations/:isoCode.json');
        }

        getActivityResource(): Resources.IActivityResourceClass {
            return <Resources.IActivityResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/activities/:id', null, {
                    update: this.updateAction,
                    getActivityTypes: this.getActivityTypesAction
                });
        }

        getRequirementNoteResource(): Resources.IBaseResourceClass<Resources.IRequirementNoteResource> {
            return <Resources.IBaseResourceClass<Resources.IRequirementNoteResource>>
                this.$resource(this.appConfig.rootUrl + '/api/requirements/:id/notes');
        }

        getCharacteristicGroupUsageResource(): Resources.ICharacteristicGroupUsageResourceClass {
            return <Resources.ICharacteristicGroupUsageResourceClass>
                this.$resource(this.appConfig.rootUrl + '/api/characteristicGroups?countryId=:countryId&propertyTypeId=:propertyTypeId');
        }

        getAttachmentResource(): Resources.IBaseResourceClass<Antares.Common.Models.Resources.IActivityAttachmentResource> {
            return <Resources.IBaseResourceClass<Antares.Common.Models.Resources.IActivityAttachmentResource>>
                this.$resource(this.appConfig.rootUrl + '/api/activities/:id/attachments');
        }

        getAzureUploadUrlResource(): ng.resource.IResourceClass<Antares.Common.Models.Resources.IAzureUploadUrlResource> {
            return <ng.resource.IResourceClass<Antares.Common.Models.Resources.IAzureUploadUrlResource>>
                this.$resource(this.appConfig.rootUrl + '/api/services/attachment/upload/activity?documentTypeId=:documentTypeId&localeIsoCode=:localeIsoCode&entityReferenceId=:entityReferenceId&filename=:filename');
        }

        getAzureDownloadUrlResource(): ng.resource.IResourceClass<Antares.Common.Models.Resources.IAzureDownloadUrlResource> {
            return <ng.resource.IResourceClass<Antares.Common.Models.Resources.IAzureDownloadUrlResource>>
                this.$resource(this.appConfig.rootUrl + '/api/services/attachment/download/activity?documentTypeId=:documentTypeId&localeIsoCode=:localeIsoCode&externalDocumentId=:externalDocumentId&entityReferenceId=:entityReferenceId&filename=:filename');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}