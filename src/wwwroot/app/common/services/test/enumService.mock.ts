/// <reference path="../../../typings/_all.d.ts" />

module Antares.Mock {
    import Dto = Common.Models.Dto;

    export class EnumServiceMock {
        private enums: any = {};

        private promise: ng.IPromise<Dto.IEnumDictionary> = null;
        private deferred: ng.IDeferred<Dto.IEnumDictionary> = null;

        constructor(private $q: ng.IQService, private dataAccessService: Antares.Services.DataAccessService) {
            this.deferred = $q.defer();
            this.promise = this.deferred.promise;
        }

        private getEnums = () =>{
            return this.enums;
        }

        public setEnum = (enumTypeCode: string, enumItems:any) => {
            this.enums[enumTypeCode] = enumItems;
        }

        public getEnumsPromise = (): ng.IPromise<Dto.IEnumDictionary> =>{
            this.deferred.resolve(this.getEnums());

            return this.promise;
        };

    }
}