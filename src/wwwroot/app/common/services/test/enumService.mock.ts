/// <reference path="../../../typings/_all.d.ts" />

module Antares.Mock {
    import Dto = Common.Models.Dto;

    export class EnumServiceMock {
        private enum: any = {};
        private enumCodeById:any = {};
        private promise: ng.IPromise<Dto.IEnumDictionary> = null;
        private deferred: ng.IDeferred<Dto.IEnumDictionary> = null;

        constructor(private $q: ng.IQService, private dataAccessService: Antares.Services.DataAccessService) {
            this.deferred = $q.defer();
            this.promise = this.deferred.promise;
        }

        private getEnum = () =>{
            return this.enum;
        }

        public setEnum = (enumTypeCode: string, enumItems:Dto.IEnumItem[]) => {
            this.enum[enumTypeCode] = enumItems;
            angular.forEach(enumItems,(item)=>{
                this.enumCodeById[item.id] = item.code;
            });
        }

        public getEnumPromise = (): ng.IPromise<Dto.IEnumDictionary> =>{
            this.deferred.resolve(this.getEnum());

            return this.promise;
        };
        
        public getEnumCodeById = (enumTypeItemId:string)=> {
            return this.enumCodeById[enumTypeItemId];
        }

    }
}