/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    import Business = Antares.Common.Models.Business;

    export class ContactViewController {
        contact: Business.Contact;

        constructor(private $state: ng.ui.IStateService) {
        }
        
        goToEdit = () => {
            this.$state.go('app.contact-edit', { id: this.$state.params['id'] });
        }
    }

    angular.module('app').controller('ContactViewController', ContactViewController);
};