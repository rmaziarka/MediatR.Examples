/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.Preview {
    import Business = Common.Models.Business;

    export class PropertyPreviewController {
        componentId: string;
        property: Business.Property = <Business.Property>{};

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private $state: ng.ui.IStateService) {

            componentRegistry.register(this, this.componentId);
        }

        setProperty = (property: Business.Property) => {
            this.property = property;
        }

        goToPropertyView = () => {
            this.$state.go('app.property-view', { id: this.property.id });
        }
    }

    angular.module('app').controller('PropertyPreviewController', PropertyPreviewController);
}