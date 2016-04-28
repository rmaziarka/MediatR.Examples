/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {

    import Dto = Common.Models.Dto;

    export class EnumService {

        private promise: ng.IPromise<Dto.IEnumDictionary> = null;

        constructor(private dataAccessService: DataAccessService) {
        }

        public init = () => {
            this.promise = this.dataAccessService.getEnumsResource().get().$promise;
        }

        public getEnumsPromise = (): ng.IPromise<Common.Models.Dto.IEnumDictionary> => {
            return this.promise;
        };
    }

    angular.module('app').service('enumService', EnumService);
}