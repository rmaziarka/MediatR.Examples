/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Common.Models {
    export module Resources {

        interface IEnumResourceParameters {
            code: string;
        }

        interface ITranslationResourceParameters {
            isoCode: string;
        }

        interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
            get(): T;
            get(params: IBaseResourceParameters): T;
            get(params: IEnumResourceParameters): T;
        }

        interface ITranslationResourceClass<T> extends IBaseResourceClass<T> {
            get(): T;
            get(params: ITranslationResourceParameters): T;
        }

        // *** IResource extensions***
        interface IActivityResource extends ng.resource.IResource<Dto.IActivity> {
        }

        interface ICompanyResource extends ng.resource.IResource<Dto.ICompany> {
        }

        interface IContactResource extends ng.resource.IResource<Dto.IContact> {
        }

        interface IRequirementResource extends ng.resource.IResource<Dto.IRequirement> {
        }

        interface IPropertyResource extends ng.resource.IResource<Dto.IProperty> {
        }

        interface ICountryLocalisedResource extends ng.resource.IResource<Dto.ICountryLocalised> {
        }

        interface IEnumResource extends ng.resource.IResource<Dto.IEnum> {
        }

        interface ICountryResource extends ng.resource.IResource<Dto.ICountryLocalised> {
        }

        interface IAddressFormResource extends ng.resource.IResource<Dto.IAddressForm> {
        }

        interface IOwnershipResource extends ng.resource.IResource<Antares.Common.Models.Dto.IOwnership> {
        }

        interface IRequirementNoteResource extends ng.resource.IResource<Antares.Common.Models.Dto.IRequirementNote> {
        }

        interface ICharacteristicGroupUsageResource extends ng.resource.IResource<Dto.ICharacteristicGroupUsage> {
        }

        // *** IResourceClass extensions ***

        // - common -
        interface IBaseResourceParameters {
            id: string;
        }
        interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
            get(): T;
            get(params: IBaseResourceParameters): T;
            update(obj:any): T;
        }

        interface IActivityResourceClass extends Resources.IBaseResourceClass<Resources.IActivityResource> {
            getActivityTypes(params: any, ownership: any): ng.resource.IResource<Dto.IActivityType>;
        }

        interface IPropertyResourceClass extends Resources.IBaseResourceClass<Resources.IPropertyResource> {
            createOwnership(params: any, ownership: any): ng.resource.IResource<Dto.IOwnership>;
            getPropertyTypes(params: any, ownership: any): ng.resource.IResource<Dto.IOwnership>;
            getAttributes(params: any, ownership: any): ng.resource.IResource<Dto.IAttribute>;
            getCharacteristics(params: any): ng.resource.IResource<Dto.ICharacteristicGroupUsage>;
        }

        // - country -
        interface ICountryLocalisedResourceParameters {
            entityTypeCode: string;
        }
        interface ICountryResourceClass extends ng.resource.IResourceClass<ICountryLocalisedResource> {
            query(): ng.resource.IResourceArray<ICountryLocalisedResource>;
            query(params: ICountryLocalisedResourceParameters): ng.resource.IResourceArray<ICountryLocalisedResource>;
        }

        // - address form -
        interface IAddressFormResourceParameters {
            entityTypeCode?: string;
            countryCode?: string;
            id?:string;
        }
        interface IAddressFormResourceClass extends ng.resource.IResourceClass<IAddressFormResource> {
            get(): IAddressFormResource;
            get(params: IAddressFormResourceParameters): IAddressFormResource;
        }

        // - characteristics
        interface ICharacteristicGroupUsageResourceParameters {
            countryId?: string;
            propertyTypeId?: string;
        }
        interface ICharacteristicGroupUsageResourceClass extends ng.resource.IResourceClass<ICharacteristicGroupUsageResource> {
            query(): ng.resource.IResourceArray<ICharacteristicGroupUsageResource>;
            query(params: ICharacteristicGroupUsageResourceParameters): ng.resource.IResourceArray<ICharacteristicGroupUsageResource>;
        }
    }
}