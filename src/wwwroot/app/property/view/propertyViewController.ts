/// <reference path="../../typings/_all.d.ts" />

module Antares.Property.View {
    import Ownership = Antares.Common.Models.Dto.IOwnership;

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
        ownershipResource: any;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private componentRegistry: Antares.Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {

            this.ownershipResource = dataAccessService.getOwnershipResource();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipAddId);
            });
        }

        showOwnershipAdd = () => {
            //TODO: Temporarily solution. Change with dynamic changing of configure button based on selection of contacts
            if (this.components.contactList().getSelected().length > 0) {
                this.components.contactSidePanel().hide();
                this.components.ownershipAdd().loadOwnership(this.components.contactList().getSelected());
                this.components.ownershipSidePanel().show();
            }
        }

        showContactList = () => {
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() => {
                    var selectedIds: string[] = [];
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() => { this.loadingContacts = false; });

            this.components.contactSidePanel().show();
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        cancelAddOwnership() {
            this.components.ownershipSidePanel().hide();
            this.components.contactSidePanel().show();
        }

        saveOwnership() {
            var ownership: Ownership = this.components.ownershipAdd().getOwnership();

            this.ownershipResource
                .save(ownership)
                .$promise
                .then((ownership) => {
                    //TODO fill ownership list
                    this.components.ownershipSidePanel().hide();
                });
        }
    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}