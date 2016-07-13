/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyViewController {
        // bindings
        tenancy: Business.TenancyViewModel;

        constructor(private $state: ng.ui.IStateService) {
        }

        goToEdit = () => {
            this.$state.go('app.tenancy-edit', { id: this.tenancy.id });
        }

    }

    angular.module('app').controller('TenancyViewController', TenancyViewController);
}
