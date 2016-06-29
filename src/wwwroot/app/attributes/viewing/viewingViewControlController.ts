/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Viewing = Common.Models.Business.Viewing;
    import Enums = Common.Models.Enums;

    export class ViewingViewControlController {
        // bindings
        viewings: Business.ViewingGroup[];
        config: IViewingViewControlConfig;
        isPanelVisible: Enums.SidePanelState;
        
        //fields
        selectedViewing: Dto.IViewing;

        constructor(private eventAggregator: Core.EventAggregator) {
        }

        showViewingPreview = (viewing: Dto.IViewing) => {
            this.selectedViewing = viewing;
            this.publishOpenEvent();
        };

        publishOpenEvent = () =>{
            this.eventAggregator.publish(new OpenViewingPrewiewPanelEvent());
        }
    }

    angular.module('app').controller('ViewingViewControlController', ViewingViewControlController);
};