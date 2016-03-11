/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            class SidePanelController {
                static $inject = ['componentRegistry'];
                private componentId: string;
                visible: boolean = false;
                constructor(componentRegistry: Antares.Core.Service.ComponentRegistry) {
                    componentRegistry.register(this, this.componentId);
                }

                show = () =>{
                    this.visible = true;
                }

                hide = () => {
                    this.visible = false;
                }
            }

            angular.module('app').controller('sidePanelController', SidePanelController);
            angular.module('app').component('sidePanel', {
                controller: 'sidePanelController',
                controllerAs: 'vm',
                templateUrl: 'app/common/components/sidePanel/sidepanel.html',
                transclude: {
                    'content': '?sidePanelContent',
                    'footer': '?sidePanelFooter'
                },
                bindings: {
                    componentId: '@'
                }
            });
        }
    }
}