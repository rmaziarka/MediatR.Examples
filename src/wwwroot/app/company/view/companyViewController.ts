/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {
    import Business = Antares.Common.Models.Business;

    export class CompanyViewController {
        company: Business.Company;

        constructor(
            private $state: ng.ui.IStateService) {
        }

        goToEdit = () => {
            this.$state.go('app.company-edit', { id: this.$state.params['id'] });
        }
    }

    angular.module('app').controller('CompanyViewController', CompanyViewController);
};