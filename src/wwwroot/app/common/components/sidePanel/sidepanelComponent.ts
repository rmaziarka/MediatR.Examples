/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            class SidePanelController {
                static $inject = ['componentRegistry'];
                static count: number = 1;
                private componentId: string;
                private _element: angular.IAugmentedJQuery;
                private elementId: string;
                constructor(componentRegistry: Antares.Core.Service.ComponentRegistry) {
                    componentRegistry.register(this, this.componentId);
                    this.elementId = 'sidePanelComponent' + (SidePanelController.count++);
                }

                getElement(){
                    return this._element || (this._element = angular.element('#' + this.elementId));
                }
                
                show = () => {
                    this.getElement().show();
                }

                hide = () => {
                    this.getElement().hide();
                }
            }

            angular.module('app').controller('sidePanelController', SidePanelController);
            angular.module('app').component('sidePanel', {
                controller: 'sidePanelController',
                controllerAs: 'vm',
                templateUrl: 'app/common/components/sidePanel/sidepanel.html',
                transclude: true,
                bindings: {
                    componentId: '@'
                }
            });
        }
    }
}