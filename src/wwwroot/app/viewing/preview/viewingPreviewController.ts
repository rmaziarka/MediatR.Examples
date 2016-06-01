///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;

        export class ViewingPreviewController {
            componentId: string;
            viewing: Business.Viewing;

            constructor(
                private componentRegistry: Antares.Core.Service.ComponentRegistry,
                private $state: ng.ui.IStateService) {
                componentRegistry.register(this, this.componentId);
            }

            goToRequirementView = () => {
                this.$state.go('app.requirement-view', { id: this.viewing.requirement.id });
            }

            goToActivityView = () => {
                this.$state.go('app.activity-view', { id: this.viewing.activity.id });
            }
        }

        angular.module('app').controller('viewingPreviewController', ViewingPreviewController);
    }
}