/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            export class SidePanelController {
                static $inject = ['componentRegistry'];
                private componentId: string;
                visible: boolean = false;
                stateChanged: boolean = false;
                constructor(componentRegistry: Antares.Core.Service.ComponentRegistry) {
                    componentRegistry.register(this, this.componentId);
                }

                show = () => {
                    if (this.visible === false) {
                        this.visible = true;
                        this.stateChanged = true;
                    }
                }

                hide = () => {
                    if (this.visible) {
                        this.visible = false;
                        this.stateChanged = true;
                    }
                }
            }

            angular.module('app').controller('sidePanelController', SidePanelController);
        }
    }
}