/// <reference path="../../typings/_all.d.ts" />

module Antares.Property.View {
    export class PropertyViewController {
        static $inject = ['dataAccessService', 'componentRegistry', '$scope'];

        componentIds: any = {
            contactListId: 'viewProperty:contactListComponent',
            contactSidePanelId: 'viewProperty:contactSidePanelComponent',
            ownershipSidePanelId: 'viewProperty:ownershipSidePanelComponent',
            ownershipAddId: 'viewProperty:ownershipAddComponent'
        }

        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
            ownershipSidePanel: () => { return this.componentRegistry.get(this.componentIds.ownershipSidePanelId); },
            ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); }
        }

        loadingContacts: boolean = false;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private componentRegistry: Antares.Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipAddId);
            });
        }

        showOwnershipAdd = () =>{
            this.components.ownershipAdd().loadOwnerships();
            this.components.ownershipSidePanel().show();
        }

        showContactList = () => {
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() => {
                    var selectedIds: string[] = [];
                    //if (this.requirement.contacts !== undefined && this.requirement.contacts !== null) {
                    //    selectedIds = <string[]>this.requirement.contacts.map(c => c.id);
                    //}
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() => { this.loadingContacts = false; });

            this.components.contactSidePanel().show();
        }

        configureContacts() {
            //this.requirement.contacts = this.components.contactList().getSelected();
            //this.components.contactSidePanel().hide();
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        cancelAddOwnership() {
            this.components.ownershipSidePanel().hide();
        }


    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}