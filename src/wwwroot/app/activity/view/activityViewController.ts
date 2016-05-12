/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;
    import Dto = Common.Models.Dto;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        activity: Business.Activity;
        attachmentsCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        enumTypeActivityDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityDocumentType;
        activityAttachmentResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityAttachmentResource>;
        saveActivityAttachmentBusy: boolean = false;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService) {

            super(componentRegistry, $scope);

            this.activityAttachmentResource = dataAccessService.getAttachmentResource();
        }

        showPropertyPreview = (property: Business.Property) => {
            this.components.propertyPreview().setProperty(property);
            this.showPanel(this.components.panels.propertyPreview);
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

        saveAttachment = (attachment: Antares.Common.Models.Business.Attachment) =>{
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

        showViewingPreview = (viewing: Common.Models.Dto.IViewing) => {
            this.components.viewingPreview().clearViewingPreview();
            this.components.viewingPreview().setViewing(viewing);
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
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
                viewingPreviewId: 'viewActivity:viewingPreviewComponent'
            };
        }

        defineComponents() {
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddId); },
                activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewId); },
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                panels: {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); },
                    activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddSidePanelId) },
                    activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewSidePanelId); },
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); }                    
                }
            };
        }
    }

    angular.module('app').controller('activityViewController', Antares.Activity.View.ActivityViewController);
}
