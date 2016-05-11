/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {

    import Dto = Common.Models.Dto;

    export class EnumService {

        private promise: ng.IPromise<Dto.IEnumDictionary> = null;

        constructor(private dataAccessService: DataAccessService) {
        }

        public init = () => {
            this.promise = this.dataAccessService.getEnumResource().get().$promise;
        }

        public getEnumPromise = (): ng.IPromise<Common.Models.Dto.IEnumDictionary> => {
            return this.promise;
        };
    }

    angular.module('app').service('enumService', EnumService);
}