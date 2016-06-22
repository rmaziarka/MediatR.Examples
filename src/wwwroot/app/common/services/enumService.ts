/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {

    import Dto = Common.Models.Dto;
    type dictionary = { [id: string]: string };

    //OBSOLETE - To remove
    // You should use EnumProvider
    export class EnumService {

        private promise: ng.IPromise<Dto.IEnumDictionary> = null;
        public enums: any;
        private enumCodeDict: dictionary = {};
        constructor(private dataAccessService: DataAccessService) {
        }

        private setEnumCode = (enumCollection:Dto.IEnumItem[]) => {
            angular.forEach(enumCollection, (value:Dto.IEnumItem) => { 
                this.enumCodeDict[value.id] = value.code;
            });
        }
        
        private mapByCode = (enumDictionary:Dto.IEnumDictionary) => {
            angular.forEach(Dto.EnumTypeCode, (enumType:string) => {
                if (enumDictionary.hasOwnProperty(enumType)) {
                    var enumCollection:Dto.IEnumItem[] = enumDictionary[enumType];
                    this.setEnumCode(enumCollection);
                }
            });
        }

        public init = () => {
            this.promise = this.dataAccessService.getEnumResource().get().$promise.then((result:Dto.IEnumDictionary) => {
                this.enums = result;
                this.mapByCode(result);
                return result;
            });
        }

        public getEnumPromise = (): ng.IPromise<Common.Models.Dto.IEnumDictionary> => {
            return this.promise;
        };
    }

    angular.module('app').service('enumService', EnumService);
}