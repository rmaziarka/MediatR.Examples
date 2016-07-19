/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ActivityRouteController {

        constructor(private $scope: ng.IScope, private activity: Dto.IActivity, private latestViewsProvider: LatestViewsProvider, config: IActivityEditViewConfig){
            $scope['config'] = config;
            $scope['activity'] = activity;

            latestViewsProvider.addView({
                entityId : activity.id,
                entityType : EntityType.Activity
            });
        }
    }

    angular.module('app').controller('ActivityRouteController', ActivityRouteController);
};