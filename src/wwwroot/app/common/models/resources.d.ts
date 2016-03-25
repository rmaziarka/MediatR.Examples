/// <reference path="../../typings/_all.d.ts" />
/// <reference path="dto/requirement.d.ts" />

declare module Antares.Common.Models {
    export module Resources {

        interface IEnumResourceParameters {
            code: string;
        }

        interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
            get(): T;
            get(params: IBaseResourceParameters): T;
            get(params: IEnumResourceParameters): T;
        }

        // *** IResource extensions***

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

        interface ICountryResource extends ng.resource.IResource<Dto.CountryLocalised> {
        }

        interface IAddressFormResource extends ng.resource.IResource<Dto.IAddressForm> {
        }

        interface IOwnershipResource extends ng.resource.IResource<Antares.Common.Models.Dto.IOwnership> {
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

        interface IPropertyResourceClass extends Resources.IBaseResourceClass<Resources.IPropertyResource> {
            createOwnership(params: any, ownership: any): ng.resource.IResource<Dto.IOwnership>;
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
    }
}