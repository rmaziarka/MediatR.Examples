/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Antares.Common.Models.Business;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        activity: Business.Activity;

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
        
        cancelActivityAttachmentAdd = () => {
            this.components.panels.activityAttachmentAdd().hide();
        };
        
        saveActivityAttachment() {
            this.components.activityAttachmentAdd().saveAttachment();            
        };        
        
        goToEdit() {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        }

        defineComponentIds(){
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent',
                activityAttachmentAddSidePanelId: 'viewActivity:activityAttachmentAddSidePanelComponent',
                activityAttachmentAddId: 'viewActivity:activityAttachmentAddComponent'
            };
        }

        defineComponents(){
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddId); },
                panels : {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); },
                    activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddSidePanelId) }
                }                
            };
        }
    }

    angular.module('app').controller('activityViewController', Antares.Activity.View.ActivityViewController);
}
