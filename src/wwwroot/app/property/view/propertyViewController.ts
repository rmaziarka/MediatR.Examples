/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/models/resources.d.ts" />

module Antares.Property.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;

    export class PropertyViewController extends Core.WithPanelsBaseController {
        ownershipAddPanelVisible: boolean = false;
        propertyId: string;

        loadingContacts: boolean = false;

        ownershipsCartListOrder: CartListOrder = new CartListOrder('purchaseDate', true, true);
        activitiesCartListOrder: CartListOrder = new CartListOrder('createdDate', true);

        property: Business.Property;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);

            this.propertyId = $state.params['id'];
            this.fixOwnershipDates();
        }

        fixOwnershipDates = () => {
            this.property.ownerships.forEach((ownership: Common.Models.Dto.IOwnership) =>{
                ownership.purchaseDate = Core.DateTimeUtils.convertDateToUtc(ownership.purchaseDate);
                ownership.sellDate = Core.DateTimeUtils.convertDateToUtc(ownership.sellDate);
            });
        }

        showOwnershipAdd = () => {
            //TODO: Temporarily solution. Change with dynamic changing of configure button based on selection of contacts
            if (this.components.contactList().getSelected().length > 0) {
                this.components.ownershipAdd().loadOwnership(this.components.contactList().getSelected());
                this.ownershipAddPanelVisible = true;
            }
        }

        showOwnershipView = (ownership: Common.Models.Dto.IOwnership) => {
            this.components.ownershipView().setOwnership(ownership);
            this.showPanel(this.components.panels.ownershipView);
        }

        showActivityAdd = () => {
            this.components.activityAdd().clearActivity();

            var vendor: Business.Ownership = _.find(this.property.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.components.activityAdd().setVendors(vendor.contacts);
            }

            this.showPanel(this.components.panels.activityAdd);
        }

        showActivityPreview = (activity: Common.Models.Business.Activity) => {
            this.components.activityPreview().setActivity(activity);
            this.showPanel(this.components.panels.activityPreview);
        }

        showContactList = () => {
            this.ownershipAddPanelVisible = false;
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() => {
                    var selectedIds: string[] = [];
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() => { this.loadingContacts = false; });

            this.components.ownershipAdd().clearOwnership();
            this.showPanel(this.components.panels.contact);
        }

        goToEdit() {
            this.$state.go('app.property-edit', { id: this.$state.params['id'] });
        }

        cancelUpdateContacts() {
            this.components.panels.contact().hide();
        }

        cancelAddOwnership() {
            this.ownershipAddPanelVisible = false;
        }

        cancelAddActivity(){
            this.components.panels.activityAdd().hide();
        }

        saveOwnership(){
            this.components.ownershipAdd().saveOwnership(this.property.id).then(() =>{
                this.cancelUpdateContacts();
            });
        }

        saveActivity(){
            this.components.activityAdd().saveActivity(this.property.id).then(() =>{
                this.cancelAddActivity();
            });
        }

        defineComponentIds() {
            this.componentIds = {
                contactListId: 'viewProperty:contactListComponent',
                contactSidePanelId: 'viewProperty:contactSidePanelComponent',
                ownershipSidePanelId: 'viewProperty:ownershipSidePanelComponent',
                ownershipAddId: 'viewProperty:ownershipAddComponent',
                ownershipViewId: 'viewProperty:ownershipViewComponent',
                activityAddId: 'viewProperty:activityAddComponent',
                activityAddSidePanelId: 'viewProperty:activityAddSidePanelComponent',
                activityPreviewId: 'viewProperty:activityPreviewComponent',
                activityPreviewSidePanelId: 'viewProperty:activityPreviewSidePanelComponent'
            };
        }

        defineComponents() {
            this.components = {
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
                activityAdd: () => { return this.componentRegistry.get(this.componentIds.activityAddId); },
                activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewId); },
                ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
                ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
                panels: {
                    contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                    ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); },
                    activityAdd: () => { return this.componentRegistry.get(this.componentIds.activityAddSidePanelId); },
                    activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('propertyViewController', PropertyViewController);
}