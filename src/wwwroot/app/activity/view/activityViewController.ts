/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    export class ActivityViewController {
        componentIds: any = {
            propertyPreviewId: 'viewActivity:propertyPreviewComponent',
            propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent'
        }

        components: any = {
            propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
            panels : {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); }
            }
        }

        currentPanel: any;

        activity: Business.Activity;

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.propertyPreviewId);
                this.componentRegistry.deregister(this.componentIds.propertyPreviewSidePanelId);
            });
        }

        showPropertyPreview = (property: Dto.Property) => {
            this.components.propertyPreview().setProperty(property);
            this.showPanel(this.components.panels.propertyPreview);
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

    angular.module('app').controller('activityViewController', Antares.Activity.View.ActivityViewController);
}
