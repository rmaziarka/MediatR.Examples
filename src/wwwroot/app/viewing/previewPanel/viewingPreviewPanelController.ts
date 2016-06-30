/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing {
    import Viewing = Antares.Common.Models.Business.Viewing;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ViewingPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        viewing: Viewing;
    }

    angular.module('app').controller('ViewingPreviewPanelController', ViewingPreviewPanelController);
}