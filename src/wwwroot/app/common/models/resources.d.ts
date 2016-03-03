/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Common.Models.Resources {
    interface IResourceParameters {
        id: number;            
    }

    interface IBaseResourceClass<T> extends ng.resource.IResourceClass<T> {
        get(): T;
        get(params: IResourceParameters): T;
    }

    interface IContactResource extends ng.resource.IResource<Dto.IContact> { }
}