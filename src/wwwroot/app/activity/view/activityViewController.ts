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

        goToEdit() {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        }

        defineComponentIds(){
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent'
            };
        }

        defineComponents(){
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                panels : {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); }
                }                
            };
        }
    }

    angular.module('app').controller('activityViewController', Antares.Activity.View.ActivityViewController);
}
