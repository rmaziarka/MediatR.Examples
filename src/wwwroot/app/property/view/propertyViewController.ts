/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/models/resources.d.ts" />

module Antares.Property.View {
    import Dto = Common.Models.Dto;
    import Resources = Common.Models.Resources;

    export class PropertyViewController {
        static $inject = ['dataAccessService', 'componentRegistry', '$scope', '$state'];

        componentIds: any = {
            contactListId: 'viewProperty:contactListComponent',
            contactSidePanelId: 'viewProperty:contactSidePanelComponent',
            ownershipSidePanelId: 'viewProperty:ownershipSidePanelComponent',
            ownershipAddId: 'viewProperty:ownershipAddComponent',
            ownershipViewId: 'viewProperty:ownershipViewComponent',
            activityAddId: 'viewProperty:activityAddComponent',
            activitySidePanelId: 'viewProperty:activitySidePanelComponent'
        }

        ownershipAddPanelVisible: boolean = false;
        propertyId: string;

        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            activityAdd: () => { return this.componentRegistry.get(this.componentIds.activityAddId); },
            ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
            ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
            panels: {
                contact : () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); },
                activity: () => { return this.componentRegistry.get(this.componentIds.activitySidePanelId); }
            }
        }

        loadingContacts: boolean = false;
        orderDescending: boolean = true;
        nullOnEnd: boolean = true;

        propertyResource: Resources.IPropertyResourceClass;
        property: Dto.Property;
        activityResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityResource>;

        currentPanel: any;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            this.propertyId = $state.params['id'];
            this.propertyResource = dataAccessService.getPropertyResource();
            this.activityResource = dataAccessService.getActivityResource();

            this.fixOwnershipDates();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipSidePanelId);
                this.componentRegistry.deregister(this.componentIds.ownershipAddId);
                this.componentRegistry.deregister(this.componentIds.ownershipViewId);
                this.componentRegistry.deregister(this.componentIds.ownershipViewSidePanelId);
                this.componentRegistry.deregister(this.componentIds.activityAddId);
            });
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
            this.showPanel(this.components.panels.activity);
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

        saveOwnership(){
            var promise = this.components.ownershipAdd().saveOwnership(this.property.id);
            promise
            .then(() => {
                    this.components.panels.contact().hide();
                });   
        }

        saveActivity() {
            // TODO implement functionality, this is just POC
            var activityStatus = this.components.activityAdd().selectedActivityStatusId;
            var activity: Common.Models.Dto.IActivity;

            activity = {
                propertyId: this.propertyId,
                activityStatusId: activityStatus.id,
                activityTypeId: activityStatus.id // todo set correct value
            };

            this.activityResource.save(activity).$promise.then((result:any) => { alert(result) });;
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

        private showPanel(panel: any) {
            this.hidePanels();
            panel().show();
            this.currentPanel = panel;
        }
    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}