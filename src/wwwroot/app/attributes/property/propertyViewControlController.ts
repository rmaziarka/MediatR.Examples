/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import PreviewProperty = Common.Models.Business.PreviewProperty;

    export class PropertyViewControlController {
        // bindings
        property: PreviewProperty;
        config: IPropertyViewControlConfig;
        hideHeader: boolean = false;
        isPanelVisible: boolean;

        constructor(private eventAggregator: Core.EventAggregator) {
        }

        publishOpenEvent = () =>{
            this.eventAggregator.publish(new OpenPropertyPrewiewPanelEvent());
        }
    }

    angular.module('app').controller('PropertyViewControlController', PropertyViewControlController);
};