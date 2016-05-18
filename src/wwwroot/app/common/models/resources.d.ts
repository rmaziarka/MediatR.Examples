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

        interface IViewingResource extends ng.resource.IResource<Dto.IViewing> {
        }

        interface IOfferResource extends ng.resource.IResource<Dto.IOffer> {
        }

        interface IPropertyResource extends ng.resource.IResource<Dto.IProperty> {
        }

        interface ICountryLocalisedResource extends ng.resource.IResource<Dto.ICountryLocalised> {
        }

        interface ICountryResource extends ng.resource.IResource<Dto.ICountryLocalised> {
        }

        interface IAddressFormResource extends ng.resource.IResource<Dto.IAddressForm> {
        }

        interface IOwnershipResource extends ng.resource.IResource<Dto.IOwnership> {
        }

        interface IRequirementNoteResource extends ng.resource.IResource<Dto.IRequirementNote> {
        }

        interface ICharacteristicGroupUsageResource extends ng.resource.IResource<Dto.ICharacteristicGroupUsage> {
        }

        interface IPropertyAreaBreakdownResource extends ng.resource.IResource<Dto.IPropertyAreaBreakdown> {
        }

        interface IActivityAttachmentResource extends ng.resource.IResource<Dto.IAttachment> {
        }

        interface IAzureUploadUrlResource extends ng.resource.IResource<Dto.IAzureUploadUrlContainer> {
        }

        interface IAzureDownloadUrlResource extends ng.resource.IResource<Dto.IAzureDownloadUrlContainer> {
        }

        interface IDepartmentUserResource extends ng.resource.IResource<Dto.IDepartmentUser> {
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

        interface IRequirementResourceClass extends Resources.IBaseResourceClass<Resources.IRequirementResource> {
        }

        interface IViewingResourceClass extends Resources.IBaseResourceClass<Resources.IViewingResource> {
            createViewing(params: any, viewing: any): ng.resource.IResource<Dto.IViewing>;
        }

        interface IOfferResourceClass extends Resources.IBaseResourceClass<Resources.IOfferResource> {
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

        // areaBreakdown
        interface IPropertyAreaBreakdownResourceClassParameters {
            propertyId: string;
        }
        interface IPropertyAreaBreakdownResourceClassData {
            areas: Dto.ICreatePropertyAreaBreakdownResource[];
        }
        interface IPropertyAreaBreakdownResourceClass extends ng.resource.IResourceClass<IPropertyAreaBreakdownResource> {
            createPropertyAreaBreakdowns(params: IPropertyAreaBreakdownResourceClassParameters, data: IPropertyAreaBreakdownResourceClassData): ng.resource.IResource<Dto.IPropertyAreaBreakdown[]>;
        }

        interface IDepartmentUserResourceParameters {
            partialName: string;
            take: string;
            'excludedIds[]': string[];
        }

        interface IDepartmentUserResourceClass extends ng.resource.IResourceClass<IDepartmentUserResource> {
            query(): ng.resource.IResourceArray<IDepartmentUserResource>;
            query(params: IDepartmentUserResourceParameters): ng.resource.IResourceArray<IDepartmentUserResource>;
        }
    }
}