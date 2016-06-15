/// <reference path="../../../typings/_all.d.ts" />

module Antares.Ownership {
    angular.module('app').component('companyContactsAddPanel', {
        templateUrl: 'app/contact/companyContact/addPanel/companyContactsAddPanel.html',
        controllerAs: 'vm',
        controller: 'CompanyContactsAddPanelController',
        transclude: true,
        bindings: {
            isVisible: '<',
            onSave: '<',
            allowMultipleSelect: '<',
            initialySelectedCompanyContacts: '<'
        }
    });
}