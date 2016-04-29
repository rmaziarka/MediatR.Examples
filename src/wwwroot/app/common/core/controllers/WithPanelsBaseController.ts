/// <reference path="../../../typings/_all.d.ts" />

module Antares.Core {
    export abstract class WithPanelsBaseController {
        // TODO change to abstract property when next version of ts is released
        // shouldn't by any type - change to specific
        components: any;
        componentIds: any;
        currentPanel: any;

        constructor(
            protected componentRegistry: Core.Service.ComponentRegistry,
            $scope: ng.IScope) {

            this.defineComponents();
            this.defineComponentIds();

            $scope.$on('$destroy', () => {
                _.each(this.componentIds, (value: string) => {
                    this.componentRegistry.deregister(value);
                });
            });
        }

        protected hidePanels(hideCurrent: boolean = true) {
            for (var panel in this.components.panels) {
                if (this.components.panels.hasOwnProperty(panel)) {
                    if (hideCurrent === false && this.currentPanel === this.components.panels[panel]()) {
                        continue;
                    }
                    this.components.panels[panel]().hide();
                }
            }
        }

        protected showPanel(panel: any) {
            this.hidePanels();
            panel().show();
            this.currentPanel = panel;
        }

        abstract defineComponents() : void;
        abstract defineComponentIds() : void;
    }
};