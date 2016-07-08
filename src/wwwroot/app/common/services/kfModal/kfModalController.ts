/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {

    export class KfModalController {

        constructor(private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
            private title: string, private message: string, private confirm:string , private cancel: string) { }

        cancelModal = () => {
            this.$uibModalInstance.dismiss();
        }

        confirmModal = () => {
            this.$uibModalInstance.close();
        }
    }

    angular.module('app').controller('KfModalController', KfModalController);
}