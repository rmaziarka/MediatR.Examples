/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;
    import Dto = Common.Models.Dto;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        activity: Business.Activity;
        attachmentsCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        enumTypeActivityDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityDocumentType;
        activityAttachmentResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityAttachmentResource>;
        saveActivityAttachmentBusy: boolean = false;
        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService,
            private latestViewsProvider: LatestViewsProvider) {

            super(componentRegistry, $scope);

            this.activityAttachmentResource = dataAccessService.getAttachmentResource();
        }

        showPropertyPreview = (property: Business.PreviewProperty) => {
            this.components.propertyPreview().setProperty(property);
            this.showPanel(this.components.panels.propertyPreview);

            this.latestViewsProvider.addViewing({
                entityId: property.id,
                entityType: EntityType.Property
            });
        }

        showActivityAttachmentAdd = () => {
            this.components.activityAttachmentAdd().clearAttachmentForm();
            this.showPanel(this.components.panels.activityAttachmentAdd);
        }

        showActivityAttachmentPreview = (attachment: Common.Models.Business.Attachment) => {
            this.components.activityAttachmentPreview().setAttachment(attachment, this.activity.id);
            this.showPanel(this.components.panels.activityAttachmentPreview);
        }

        cancelActivityAttachmentAdd = () => {
            this.components.panels.activityAttachmentAdd().hide();
        };

        saveAttachment = (attachment: Common.Models.Business.Attachment) =>{
            return this.activityAttachmentResource.save({ id : this.activity.id }, new Business.CreateActivityAttachmentResource(this.activity.id, attachment))
                .$promise;
        }

        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.activity.attachments.push(savedAttachment);

            this.hidePanels(true);
        }

        saveActivityAttachment = () => {
            this.saveActivityAttachmentBusy = true;

            this.components.activityAttachmentAdd()
                .uploadAttachment(this.activity.id)
                .then(this.saveAttachment)
                .then(this.addSavedAttachmentToList)
                .finally(() =>{
                     this.saveActivityAttachmentBusy = false;
                });
        };

        showViewingPreview = (viewing: Common.Models.Dto.IViewing) =>{
            this.selectedViewing = viewing;
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        }

        showOfferPreview = (offer: Common.Models.Dto.IOffer) => {
            this.selectedOffer = offer;
            this.showPanel(this.components.panels.offerPreview);
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        }

        navigateToOfferView = (offer: Common.Models.Dto.IOffer) =>{
            this.$state.go('app.offer-view', { id: offer.id });
        }

        defineComponentIds() {
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent',
                activityAttachmentAddSidePanelId: 'viewActivity:activityAttachmentAddSidePanelComponent',
                activityAttachmentAddId: 'viewActivity:activityAttachmentAddComponent',
                activityAttachmentPreviewId: 'viewActivity:activityyAttachmentPreviewComponent',
                activityAttachmentPreviewSidePanelId: 'viewActivity:activityyAttachmentPreviewSidePanelComponent',
                previewViewingSidePanelId: 'viewActivity:previewViewingSidePanelComponent',
                viewingPreviewId: 'viewActivity:viewingPreviewComponent',
                offerPreviewId: 'viewActivity:offerPreviewComponent',
                offerPreviewSidePanelId: 'viewActivity:offerPreviewSidePanelComponent'
            };
        }

        defineComponents() {
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddId); },
                activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewId); },
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                offerPreview: () => { return this.componentRegistry.get(this.componentIds.offerPreviewId);  },
                panels: {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); },
                    activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddSidePanelId) },
                    activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewSidePanelId); },
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
                    offerPreview: () =>{ return this.componentRegistry.get(this.componentIds.offerPreviewSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('activityViewController', ActivityViewController);
}
