/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    export class KfModalService {

        defaultConfirmCode: string = 'COMMON.MODALS.CONFIRM';
        defaultCancelCode: string = 'COMMON.MODALS.CANCEL';

        constructor(
            private $translate: ng.translate.ITranslateService,
            private $uibModal: angular.ui.bootstrap.IModalService) {
        }

        showModal(titleCode: string, messageCode: string, confirmCode?: string, cancelCode?: string): ng.IPromise<void>{
            confirmCode = confirmCode || this.defaultConfirmCode;
            cancelCode = cancelCode || this.defaultCancelCode;

            var modalInstance = this.$uibModal.open({
                templateUrl : 'app/common/services/kfModal/kfModal.html',
                controller: 'KfModalController',
                controllerAs : "vm",
                resolve : {
                    title : () => this.$translate.instant(titleCode),
                    message: () => this.$translate.instant(messageCode),
                    confirm: () => this.$translate.instant(confirmCode),
                    cancel: () => this.$translate.instant(cancelCode)
                }
            });

            return modalInstance.result;
        }
    }

    angular.module('app').service('kfModalService', KfModalService);
}