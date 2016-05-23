/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    export class KfMessageService {

        constructor(
            private growl: angular.growl.IGrowlService,
            private $filter: ng.IFilterService) {
        }

        showSuccess(message: string): angular.growl.IGrowlMessage {
            return this.growl.success(message);
        }

        showSuccessByCode(messageCode: string): angular.growl.IGrowlMessage {
            var message: string = this.$filter('translate')(messageCode);
            return this.growl.success(message);
        }
    }

    angular.module('app').service('kfMessageService', KfMessageService);
}