/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    export class RequirementAddController {
        static $inject = ['dataAccessService', 'componentRegistry', '$scope'];
        componentIds: any = {
            contactListId: 'addRequirement:contactListComponent',
            contactSidePanelId: 'addRequirement:contactSidePanelComponent',
        }
        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); }
        }
        requirementResource: any;
        requirement: any = {};
        loadingContacts: boolean = false;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private componentRegistry: Antares.Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {
            this.requirementResource = dataAccessService.getRequirementResource();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
            });
        }

        updateContacts() {
            this.requirement.contacts = this.components.contactList().getSelected();
            this.components.contactSidePanel().hide();
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        showContactList = () =>{
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() =>{
                    var selectedIds: string[] = [];
                    if (this.requirement.contacts !== undefined && this.requirement.contacts !== null) {
                        selectedIds = <string[]>this.requirement.contacts.map(c => c.id);
                    }
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() =>{ this.loadingContacts = false; });
            
            this.components.contactSidePanel().show();
        }

        public save() {
            this.requirementResource.save(this.requirement);
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}