/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    export class RequirementAddController {
        static $inject = ['componentRegistry'];
        contacts: Array<any> = [];
        componentIds: any = {
            contactListId: 'addRequirement:contactListComponent',
            contactSidePanelId: 'addRequirement:contactSidePanelComponent',
            
        }
        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); }
        }
        
        constructor(private componentRegistry) {
        }

        updateContacts() {
            this.contacts = this.components.contactList().getSelected();
            this.components.contactSidePanel().hide();            
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        showContactList = () => {
            this.components.contactSidePanel().show();
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}