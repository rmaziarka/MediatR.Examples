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
            contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
            ownershipSidePanel: () => { return this.componentRegistry.get(this.componentIds.ownershipSidePanelId); },
            ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
            ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
            ownershipViewSidePanel: () => { return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); }
        }

        loadingContacts: boolean = false;
        ownershipResource: any;
        property: Antares.Common.Models.Resources.IPropertyResource;

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
                this.components.contactSidePanel().hide();
                this.components.ownershipAdd().loadOwnership(this.components.contactList().getSelected());
                this.components.ownershipSidePanel().show();
            }
        }

        loadPropertyData = () =>{
            var propertyId: string = this.$state.params.id;

            this.property = this.dataAccessService.getPropertyResource().get({ id: propertyId });
        }

        showOwnershipView = (ownership) => {
            this.components.ownershipView().setOwnership(ownership);
            if (this.components.contactSidePanel().visible) {
                this.components.contactSidePanel().hide();
            }
            if (this.components.ownershipSidePanel().visible) {
                this.components.ownershipSidePanel().hide();
            }
            this.components.ownershipViewSidePanel().show();
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

            if (this.components.ownershipViewSidePanel().visible) {
                this.components.ownershipViewSidePanel().hide();
            }

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
            var ownershipToSend = this.getOwnershipToSave();

            this.ownershipResource
                .save(ownershipToSend)
                .$promise
                .then((ownership) => {
                    //TODO fill ownership list
                    this.components.ownershipSidePanel().hide();
                });
        }

        getOwnershipToSave(){
            var ownership = angular.copy(this.components.ownershipAdd().getOwnership());
            ownership.ContactIds = ownership.contacts.map((item) => { return item.id; });
            ownership.PropertyId = null;
            ownership.OwnershipTypeId = ownership.ownershipType.id;
            delete ownership.contacts;
            delete ownership.ownershipType;

            return ownership;
        }
    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}