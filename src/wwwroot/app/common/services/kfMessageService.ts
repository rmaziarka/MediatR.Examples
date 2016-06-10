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

        showError(message: string): angular.growl.IGrowlMessage {
            return this.growl.error(message);
        }

        showErrorByCode(messageCode: string, titleCode?: string): angular.growl.IGrowlMessage {
            var message: string = this.$filter('translate')(messageCode);

            if (titleCode) {
                var title: string = this.$filter('translate')(titleCode);
                return this.growl.error(message, {title: title });
            }
            else {
                return this.growl.error(message);
            }
        }

        showErrors(response: any): Array<angular.growl.IGrowlMessage> {
            var result: Array<angular.growl.IGrowlMessage> = [];
            var errors: Array<any> = (response && response.data && response.data instanceof Array) ? response.data : [];
            errors.forEach((error) => {
                result.push(this.growl.error(error.message));
            });
            return result;
        }
    }

    angular.module('app').service('kfMessageService', KfMessageService);
}