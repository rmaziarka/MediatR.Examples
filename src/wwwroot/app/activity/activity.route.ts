/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import IActivity = Common.Models.Dto.IActivity;
    import PageTypeEnum = Common.Models.Enums.PageTypeEnum;
    var app: ng.IModule = angular.module('app');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.activity-add', {
                url: '/property/:propertyId/activity/add',
                template: "<activity-add property='property'></activity-add>",
                controller: ($scope: ng.IScope, property: Common.Models.Dto.IProperty) => {
                    $scope['property'] = new Common.Models.Business.PropertyView(property);
                },
                resolve: {
                    property: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var propertyId: string = $stateParams['propertyId'];
                        return dataAccessService.getPropertyResource().get({ id: propertyId }).$promise;
                    }
                }
            })
            .state('app.activity-edit', {
                url: '/activity/edit/:id',
                template: "<activity-edit activity='activity' config='config'></activity-edit>",
                controller: "ActivityRouteController",
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    editConfig: (activity: IActivity, configService: Services.ConfigService) => {
                        return configService.getActivity(PageTypeEnum.Update,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            activity);
                    },
                    viewConfig: (activity: IActivity, configService: Services.ConfigService) => {
                        return configService.getActivity(PageTypeEnum.Details,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            activity);
                    },
                    config: (editConfig: IActivityEditConfig, viewConfig: IActivityViewConfig) => {
                        var config: IActivityEditConfig = <IActivityEditConfig>({});

                        Object.keys(editConfig)
                            .forEach((key: string) => {
                                config[key] = editConfig[key];
                            });

                        Object.keys(viewConfig)
                            .forEach((key: string) => {
                                if (config[key]) {
                                    return;
                                }

                                config[key] = viewConfig[key];
                            });
                        return config;
                    }
                }
            })
            .state('app.activity-view', {
                url: '/activity/view/:id',
                template: "<activity-view activity='activity' config='config'></activity-view>",
                controller: "ActivityRouteController",
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    config: (activity: IActivity, configService: Services.ConfigService) => {
                        return configService.getActivity(PageTypeEnum.Details,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            activity);
                    }
                }
            });
    }
}