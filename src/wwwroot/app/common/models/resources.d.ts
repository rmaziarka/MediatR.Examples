/// <reference path="../../typings/_all.d.ts" />
/// <reference path="dto/requirement.d.ts" />

declare module Antares.Common.Models {
    export module Resources {
        interface IResourceParameters {
            id: string;
        }

        interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
            get(): T;
            get(params: IResourceParameters): T;
        }

        interface IContactResource extends ng.resource.IResource<Dto.IContact> {
        }

        interface IContactResource extends ng.resource.IResource<Antares.Common.Models.Dto.IContact> { }

        interface IRequirementResource extends ng.resource.IResource<Antares.Common.Models.Dto.IRequirement> { }

        interface IOwnershipResource extends ng.resource.IResource<Antares.Common.Models.Dto.IOwnership> { }
    }
}