/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;
    import Dto = Common.Models.Dto;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        activity: Business.Activity;
        attachmentsCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        enumTypeActivityDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityDocumentType;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);
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
            this.components.activityAttachmentPreview().setAttachment(attachment);
            this.showPanel(this.components.panels.activityAttachmentPreview);
        }

        cancelActivityAttachmentAdd = () => {
            this.components.panels.activityAttachmentAdd().hide();
        };

        saveActivityAttachment = () => {
            this.components.activityAttachmentAdd().saveAttachment();
        };

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        }

        defineComponentIds(){
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent',
                activityAttachmentAddSidePanelId: 'viewActivity:activityAttachmentAddSidePanelComponent',
                activityAttachmentAddId: 'viewActivity:activityAttachmentAddComponent',
                activityAttachmentPreviewId: 'viewActivity:activityyAttachmentPreviewComponent',
                activityAttachmentPreviewSidePanelId: 'viewActivity:activityyAttachmentPreviewSidePanelComponent'
            };
        }

        defineComponents(){
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddId); },
                activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewId); },
                panels : {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); },
                    activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddSidePanelId) },
                    activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('activityViewController', Antares.Activity.View.ActivityViewController);
}
