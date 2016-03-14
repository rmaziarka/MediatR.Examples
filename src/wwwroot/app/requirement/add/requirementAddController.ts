/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    export class RequirementAddController {
        static $inject = ['dataAccessService', 'componentRegistry'];
        componentIds: any = {
            contactListId : 'addRequirement:contactListComponent',
            contactSidePanelId : 'addRequirement:contactSidePanelComponent',
        }
        components: any = {
            contactList : () =>{ return this.componentRegistry.get(this.componentIds.contactListId); },
            contactSidePanel : () =>{ return this.componentRegistry.get(this.componentIds.contactSidePanelId); }
        }
        requirementResource: any;
        requirement: any = {};

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private componentRegistry) {
            this.requirementResource = dataAccessService.getRequirementResource();
        }

        updateContacts() {
            this.requirement.contacts = this.components.contactList().getSelected();
            this.components.contactSidePanel().hide();
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        showContactList = () => {
            this.components.contactSidePanel().show();
        }

        public save() {
            this.requirementResource.save(this.requirement);
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}