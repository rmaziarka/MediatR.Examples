/// <reference path="../../typings/_all.d.ts" />

module Antares.Property.View {
    import Ownership = Antares.Common.Models.Dto.IOwnership;

    export class PropertyViewController {
        static $inject = ['dataAccessService', 'componentRegistry', '$scope', '$state'];

        componentIds: any = {
            contactListId: 'viewProperty:contactListComponent',
            contactSidePanelId: 'viewProperty:contactSidePanelComponent',
            ownershipSidePanelId: 'viewProperty:ownershipSidePanelComponent',
            ownershipAddId: 'viewProperty:ownershipAddComponent',
            ownershipViewSidePanelId: 'viewProperty:ownershipViewSidePanelComponent',
            ownershipViewId: 'viewProperty:ownershipViewComponent'
        }

        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
            ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
            panels: {
                contact : () =>{ return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                ownership : () =>{ return this.componentRegistry.get(this.componentIds.ownershipSidePanelId); },
                ownershipView : () =>{ return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); },
            }
        }

        loadingContacts: boolean = false;
        ownershipResource: any;
        property: Antares.Common.Models.Resources.IPropertyResource;
        currentPanel: any;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private componentRegistry: Antares.Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IState) {

            this.ownershipResource = dataAccessService.getOwnershipResource();

            this.loadPropertyData();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipAddId);
                this.componentRegistry.deregister(this.componentIds.ownershipViewId);
                this.componentRegistry.deregister(this.componentIds.ownershipViewSidePanelId);
            });
        }

        showOwnershipAdd = () => {
            //TODO: Temporarily solution. Change with dynamic changing of configure button based on selection of contacts
            if (this.components.contactList().getSelected().length > 0) {
                this.components.ownershipAdd().loadOwnership(this.components.contactList().getSelected());
                this.showPanel(this.components.panels.ownership);
            }
        }

        loadPropertyData = () => {
            var propertyId: string = this.$state.params.id;
            this.property = this.dataAccessService.getPropertyResource().get({ id: propertyId });
        }

        showOwnershipView = (ownership) => {
            this.components.ownershipView().setOwnership(ownership);
            this.showPanel(this.components.panels.ownershipView);
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

            this.showPanel(this.components.panels.contact);
        }

        cancelUpdateContacts() {
            this.components.panels.contact().hide();
        }

        cancelAddOwnership(){
            this.showPanel(this.components.panels.contact);
        }

        saveOwnership() {
            var ownershipToSend = this.getOwnershipToSave();

            this.ownershipResource
                .save(ownershipToSend)
                .$promise
                .then((ownership: any) => {
                    //TODO fill ownership list
                    this.components.panels.ownership().hide();
                });
        }

        getOwnershipToSave() {
            var ownership = angular.copy(this.components.ownershipAdd().getOwnership());
            ownership.ContactIds = ownership.contacts.map((item: any) => { return item.id; });
            ownership.PropertyId = null;
            ownership.OwnershipTypeId = ownership.ownershipType.id;
            delete ownership.contacts;
            delete ownership.ownershipType;

            return ownership;
        }

        private hidePanels(hideCurrent: boolean = true) {
            for (var panel in this.components.panels) {
                if (this.components.panels.hasOwnProperty(panel)) {
                    if (hideCurrent === false && this.currentPanel === this.components.panels[panel]()) {
                        continue;
                    }
                    this.components.panels[panel]().hide();
                }
            }
        }

        private showPanel(panel) {
            this.hidePanels();
            panel().show();
            this.currentPanel = panel;
        }
    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}