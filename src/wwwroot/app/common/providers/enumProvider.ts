/// <reference path="../../typings/_all.d.ts" />

module Antares.Providers {

    import Dto = Common.Models.Dto;
    import DataAccessService = Services.DataAccessService;
    type Dictionary = { [id: string]: string };
    export class EnumProvider {
        
        public enums: Dto.IEnumDictionary;
        private enumCodeDict: Dictionary = {};
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
            return this.dataAccessService.getEnumResource().get().$promise.
                then((result: Dto.IEnumDictionary) => {
                    this.enums = result;
                    this.mapByCode(result);
                    return result;
                });
        }

        public getEnumCodeById = (enumId: string) : string => {
            return this.enumCodeDict[enumId];
        }
    }

    angular.module('app').service('enumProvider', EnumProvider);
}