/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            export class SidePanelController {
                private componentId: string;
                isBusy: boolean = false;

                isFooterVisible: boolean = false;
                isHeaderVisible: boolean = false;

                visible: boolean = false;
                stateChanged: boolean = false;

                constructor(componentRegistry: Antares.Core.Service.ComponentRegistry, $transclude: any /* There is missing isSlotFilled definition in ng.ITranscludeFunction. */) {
                    componentRegistry.register(this, this.componentId);

                    this.isFooterVisible = $transclude.isSlotFilled('footer');
                    this.isHeaderVisible = $transclude.isSlotFilled('header');
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