/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/models/resources.d.ts" />

module Antares.Property.View {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import CartListOrder = Common.Component.ListOrder;
    import Resources = Common.Models.Resources;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class PropertyViewController extends Core.WithPanelsBaseController {
        isActivityAddPanelVisible: boolean;

        ownershipAddPanelVisible: boolean = false;
        propertyId: string;

        loadingContacts: boolean = false;

        ownershipsCartListOrder: CartListOrder = new CartListOrder('purchaseDate', true, true);
        activitiesCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        userData: Dto.IUserData;
        property: Business.PropertyView;
        config: Activity.IActivityAddPanelConfig;
        savePropertyActivityBusy: boolean = false;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private latestViewsProvider: LatestViewsProvider,
            private eventAggregator: Antares.Core.EventAggregator) {

            super(componentRegistry, $scope);
            this.propertyId = $state.params['id'];
            this.fixOwnershipDates();

            this.eventAggregator.with(this).subscribe(Antares.Common.Component.CloseSidePanelEvent, () => {
                // TODO iteration?
                this.isActivityAddPanelVisible = false;
            });

            this.eventAggregator.with(this).subscribe(Activity.ActivityAddedSidePanelEvent, (msg: Activity.ActivityAddedSidePanelEvent) => {
                this.property.activities.push(new Business.Activity(msg.activityAdded));
            });
        }

        onPanelsHidden = () =>{
            this.isActivityAddPanelVisible = false;
        }

        fixOwnershipDates = () => {
            this.property.ownerships.forEach((ownership: Dto.IOwnership) => {
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

        showOwnershipView = (ownership: Dto.IOwnership) => {
            this.components.ownershipView().setOwnership(ownership);
            this.showPanel(this.components.panels.ownershipView);
        }

        showActivityAdd = () =>{
            this.hidePanels();
            this.isActivityAddPanelVisible = true;
        }

        showAreaAdd = () => {
            this.components.areaAdd().clearAreas();
            this.showPanel(this.components.panels.areaAdd);
        }

        showAreaEdit = (propertyAreaBreakdown: Business.PropertyAreaBreakdown) => {
            this.components.areaEdit().editPropertyAreaBreakdown(propertyAreaBreakdown);
            this.showPanel(this.components.panels.areaEdit);
        }

        showActivityPreview = (activity: Common.Models.Business.Activity) => {
            this.components.activityPreview().setActivity(activity);
            this.showPanel(this.components.panels.activityPreview);

            this.latestViewsProvider.addView({
                entityId: activity.id,
                entityType: EntityType.Activity
            });
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

        cancelAddActivity() {
            this.components.panels.activityAdd().hide();
        }

        cancelAddArea() {
            this.components.panels.areaAdd().hide();
        }

        cancelEditArea() {
            this.components.panels.areaEdit().hide();
        }

        saveOwnership() {
            this.components.ownershipAdd().saveOwnership(this.property.id).then(() => {
                this.cancelUpdateContacts();
            });
        }

        saveActivity() {
            this.savePropertyActivityBusy = true;

            this.components.activityAdd().saveActivity(this.property.id).then((result: Dto.IActivity) => {
                this.cancelAddActivity();

                this.latestViewsProvider.addView({
                    entityId: result.id,
                    entityType: EntityType.Activity
                });
            }).finally(() => {
                this.savePropertyActivityBusy = false;
            });
        }

        saveArea() {
            this.components.areaAdd().saveAreas(this.property.id).then((areas: Business.PropertyAreaBreakdown[]) => {
                [].push.apply(this.property.propertyAreaBreakdowns, areas);
                this.cancelAddArea();
            });
        }

        updateArea() {
            this.components.areaEdit().updatePropertyAreaBreakdown(this.property.id).then((area: Business.PropertyAreaBreakdown) => {
                var index = _.findIndex(this.property.propertyAreaBreakdowns, (item) => { return item.id === area.id });
                if (index >= 0) {
                    var currentArea = this.property.propertyAreaBreakdowns[index];
                    angular.extend(currentArea, area);
                    this.cancelEditArea();
                }
            });
        }

        areaBreakdownDndOptions: Common.Models.IDndOptions = {
            dragEnd: this.onAreaDraggedAndDropped()
        }

        onAreaDraggedAndDropped() {
            return (event: Common.Models.IDndEvent) => {
                var movedItem: Business.PropertyAreaBreakdown = event.source.itemScope.modelValue;
                movedItem.order = event.dest.index;

                var params: Resources.IPropertyAreaBreakdownResourceClassParameters = { propertyId: this.property.id };
                var data = new Business.UpdatePropertyAreaBreakdownOrderResource(movedItem, this.property.id);

                this.dataAccessService.getPropertyAreaBreakdownResource()
                    .updatePropertyAreaBreakdownOrder(params, data)
                    .$promise.then((response: Dto.IPropertyAreaBreakdown[]) => {
                        var areas = response.map((r: Dto.IPropertyAreaBreakdown) => new Business.PropertyAreaBreakdown(<Dto.IPropertyAreaBreakdown>r));
                        angular.extend(event.source.sortableScope.modelValue, areas);
                    });
            }
        }

        defineComponentIds() {
            this.componentIds = {
                contactListId: 'viewProperty:contactListComponent',
                contactSidePanelId: 'viewProperty:contactSidePanelComponent',
                ownershipSidePanelId: 'viewProperty:ownershipSidePanelComponent',
                ownershipAddId: 'viewProperty:ownershipAddComponent',
                ownershipViewId: 'viewProperty:ownershipViewComponent',
                ownershipViewSidePanelId: 'viewProperty:ownershipViewSidePanelComponent',
                activityAddId: 'viewProperty:activityAddComponent',
                activityAddSidePanelId: 'viewProperty:activityAddSidePanelComponent',
                activityPreviewId: 'viewProperty:activityPreviewComponent',
                activityPreviewSidePanelId: 'viewProperty:activityPreviewSidePanelComponent',
                areaAddSidePanelId: 'viewProperty:areaAddSidePanelComponent',
                areaEditSidePanelId: 'viewProperty:areaEditSidePanelComponent',
                areaAddId: 'viewProperty:areaAddComponent',
                areaEditId: 'viewProperty:areaEditComponent'
            };
        }

        defineComponents() {
            this.components = {
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
                activityAdd: () => { return this.componentRegistry.get(this.componentIds.activityAddId); },
                activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewId); },
                ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
                ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
                areaAdd: () => { return this.componentRegistry.get(this.componentIds.areaAddId); },
                areaEdit: () => { return this.componentRegistry.get(this.componentIds.areaEditId); },
                panels: {
                    contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                    ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); },
                    activityAdd: () => { return this.componentRegistry.get(this.componentIds.activityAddSidePanelId); },
                    activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewSidePanelId); },
                    areaAdd: () => { return this.componentRegistry.get(this.componentIds.areaAddSidePanelId); },
                    areaEdit: () => { return this.componentRegistry.get(this.componentIds.areaEditSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('propertyViewController', PropertyViewController);
}