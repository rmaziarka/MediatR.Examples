/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing {
    import Viewing = Antares.Common.Models.Business.Viewing;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ViewingPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {

        constructor(private latestViewsProvider: LatestViewsProvider){
            super();
        }

        // binding
        viewing: Viewing;

        panelShown = () => {
            this.latestViewsProvider.addView({
                entityId: this.viewing.id,
                entityType: EntityType.Activity
            });
        }
    }

    angular.module('app').controller('ViewingPreviewPanelController', ViewingPreviewPanelController);
}