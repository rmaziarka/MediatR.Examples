/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    export class RequirementAddController {
        static $inject = ['componentRegistry'];
        contacts:Array<any> = [];
        showSidePanel: boolean = false;
        
        updateContacts(){
            this.showSidePanel = false;
            var contactsList = this.componentRegistry.get('testId');
            this.contacts = contactsList.getSelected();
        }

        cancelUpdateContacts(){
            this.showSidePanel = false;
        }
        
        constructor(private componentRegistry) {
        }

        showContactList = () => {
            this.showSidePanel = true;
        }
    }

    angular.module('app').controller('requirementAddController', RequirementAddController);
}