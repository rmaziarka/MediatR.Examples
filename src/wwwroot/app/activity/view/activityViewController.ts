/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    export class ActivityViewController {
        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {
        }

        showPropertyPreview = () => {
        }
    }

    angular.module('app').controller('activityViewController', ActivityViewController);
}
