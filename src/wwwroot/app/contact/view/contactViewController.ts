/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    import Business = Antares.Common.Models.Business;

    export class ContactViewController {
        contact: Business.Contact;

        constructor(private $state: ng.ui.IStateService) {
        }
        
        goToEdit = () => {
            
        }
    }

    angular.module('app').controller('ContactViewController', ContactViewController);
};