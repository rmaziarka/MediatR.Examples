/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;
    import PreviewProperty = Antares.Common.Models.Business.PreviewProperty;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class PropertyPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {

        constructor(private latestViewsProvider: LatestViewsProvider){
            super();
        }

        // binding
        property: PreviewProperty;

        panelShown = () => {
            this.latestViewsProvider.addView({
                entityId: this.property.id,
                entityType: EntityType.Property
            });
        }
    }

    angular.module('app').controller('PropertyPreviewPanelController', PropertyPreviewPanelController);
}