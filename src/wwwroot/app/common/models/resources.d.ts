/// <reference path="../../typings/_all.d.ts" />
/// <reference path="dto/requirement.d.ts" />

declare module Antares.Common.Models {
    export module Resources {
        interface IResourceParameters {
            id: string;
        }

        interface IEnumResourceParameters {
            code: string;
        }

        interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
            get(): T;
            get(params: IResourceParameters): T;
            get(params: IEnumResourceParameters): T;
        }

        interface IContactResource extends ng.resource.IResource<Dto.IContact> {
        }

        interface IRequirementResource extends ng.resource.IResource<Dto.IRequirement> {

        }

        interface IEnumResource extends ng.resource.IResource<Dto.IEnum> {

        }
        
        interface ICountryResource extends ng.resource.IResource<Dto.CountryLocalised> {
        }

        interface ICountryResourceParameters {
            entityTypeCode: string;
        }

        interface ICountryResourceClass extends ng.resource.IResourceClass<ICountryResource> {
            query(): ng.resource.IResourceArray<ICountryResource>; // without this line interface doesn't compile !!!????????????????
            query(params: ICountryResourceParameters): ng.resource.IResourceArray<ICountryResource>;
        }


        interface IAddressFormResource extends ng.resource.IResource<Dto.AddressForm> {
        }

        interface IAddressFormResourceParameters {
            entityTypeCode: string;
            countryCode: string;
        }

        interface IAddressFormResourceClass extends ng.resource.IResourceClass<IAddressFormResource> {
            get(): IAddressFormResource; // without this line interface doesn't compile !!!????????????????
            get(params: IAddressFormResourceParameters): IAddressFormResource;
        }

        interface IOwnershipResource extends ng.resource.IResource<Antares.Common.Models.Dto.IOwnership> { }
    }
}