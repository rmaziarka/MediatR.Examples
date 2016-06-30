/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/models/resources.d.ts" />

module Antares.Property.View {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import CartListOrder = Common.Component.ListOrder;
    import Resources = Common.Models.Resources;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import Attachment = Common.Component.Attachment;

    export class PropertyViewController extends Core.WithPanelsBaseController {

        isActivityAddPanelVisible:Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isActivityPreviewPanelVisible:Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAttachmentsUploadPanelVisible:Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAttachmentsPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        ownershipAddPanelVisible: boolean = false;
        
        propertyId: string;


        loadingContacts: boolean = false;

        ownershipsCartListOrder: CartListOrder = new CartListOrder('purchaseDate', true, true);
        activitiesCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        userData: Dto.ICurrentUser;
        property: Business.PropertyView;
        config: Activity.IActivityAddPanelConfig;
        savePropertyActivityBusy: boolean = false;
        selectedActivity: Common.Models.Business.Activity;
        attachmentManagerData: Attachment.IAttachmentsManagerData;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private propertyService: Services.PropertyService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private latestViewsProvider: LatestViewsProvider,
            private eventAggregator: Core.EventAggregator) {

            super(componentRegistry, $scope);
            this.propertyId = $state.params['id'];
            this.fixOwnershipDates();

            this.eventAggregator.with(this).subscribe(Attachment.OpenAttachmentPreviewPanelEvent, this.openAttachmentPreviewPanel);
            this.eventAggregator.with(this).subscribe(Attachment.OpenAttachmentUploadPanelEvent, this.openAttachmentUploadPanel);

            this.eventAggregator.with(this).subscribe(Activity.ActivityAddedSidePanelEvent, (msg: Activity.ActivityAddedSidePanelEvent) => {
                this.property.activities.push(new Business.Activity(msg.activityAdded));
            });

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () =>{
                this.hidePanels();
            });

            eventAggregator
                .with(this)
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                    this.recreateAttachmentsData();
                });

            this.recreateAttachmentsData();
        }

        onOldPanelsHidden = () =>{
            this.hideNewPanels();
        }

        hideNewPanels = () =>{
            this.isActivityAddPanelVisible = Enums.SidePanelState.Closed;
            this.isActivityPreviewPanelVisible = Enums.SidePanelState.Closed;
            this.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Closed;
            this.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Closed;

            this.recreateAttachmentsData();
        }

        openAttachmentPreviewPanel = () => {
            this.hidePanels();

            this.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Opened;
            this.recreateAttachmentsData();
        }

        openAttachmentUploadPanel = () => {
            this.hidePanels();

            this.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Opened;
            this.recreateAttachmentsData();
        }

        recreateAttachmentsData = () => {
            this.attachmentManagerData = {
                entityId: this.property.id,
                enumDocumentType : Dto.EnumTypeCode.PropertyDocumentType,
                entityType: Enums.EntityTypeEnum.Property,
                attachments: this.property.attachments,
                isPreviewPanelVisible: this.isAttachmentsPreviewPanelVisible,
                isUploadPanelVisible: this.isAttachmentsUploadPanelVisible
            }
        }
 
        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.property.attachments.push(savedAttachment);
        }

        saveAttachment = (attachment: Common.Component.Attachment.AttachmentUploadCardModel) => {
            var command = new Property.Command.PropertyAttachmentSaveCommand(this.property.id, attachment);
            return this.propertyService.createPropertyAttachment(command)
                .then((result: angular.IHttpPromiseCallbackArg<Dto.IAttachment>) => { return result.data; });
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
            this.isActivityAddPanelVisible = Enums.SidePanelState.Opened;
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
            this.hidePanels();
            this.selectedActivity = activity;
            this.isActivityPreviewPanelVisible = Enums.SidePanelState.Opened;
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
                areaAddSidePanelId: 'viewProperty:areaAddSidePanelComponent',
                areaEditSidePanelId: 'viewProperty:areaEditSidePanelComponent',
                areaAddId: 'viewProperty:areaAddComponent',
                areaEditId: 'viewProperty:areaEditComponent'
            };
        }

        defineComponents() {
            this.components = {
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
                ownershipAdd: () => { return this.componentRegistry.get(this.componentIds.ownershipAddId); },
                ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewId); },
                areaAdd: () => { return this.componentRegistry.get(this.componentIds.areaAddId); },
                areaEdit: () => { return this.componentRegistry.get(this.componentIds.areaEditId); },
                panels: {
                    contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                    ownershipView: () => { return this.componentRegistry.get(this.componentIds.ownershipViewSidePanelId); },
                    areaAdd: () => { return this.componentRegistry.get(this.componentIds.areaAddSidePanelId); },
                    areaEdit: () => { return this.componentRegistry.get(this.componentIds.areaEditSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('propertyViewController', PropertyViewController);
}