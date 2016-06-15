/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import IActivity = Antares.Common.Models.Dto.IActivity;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;
    var app: ng.IModule = angular.module('app');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.activity-view', {
                url: '/activity/view/:id',
                template: "<activity-view activity='activity' config='config'></activity-view>",
                controller: "ActivityRouteController",
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    config: (activity: IActivity, configService: Services.ConfigService) =>{
                        return configService.getActivity(PageTypeEnum.Details,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            activity);
                    }
                }
            })
            .state('app.activity-edit', {
                url: '/activity/edit/:id',
                template: "<activity-edit activity='activity'></activity-edit>",
                controller: "ActivityRouteController",
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    config: (activity: IActivity, configService: Services.ConfigService) => {
                        return configService.getActivity(PageTypeEnum.Update,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            activity);
                    }
                }
            });
    }
}