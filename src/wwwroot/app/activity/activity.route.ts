/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import IActivity = Common.Models.Dto.IActivity;
    import PageTypeEnum = Common.Models.Enums.PageTypeEnum;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Commands = Common.Models.Commands;

    var app: ng.IModule = angular.module('app');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.activity-add', {
                url: '/property/:propertyId/activity/add',
                template: "<activity-add activity='activity' user-data='appVm.userData'></activity-add>",
                controller: ($scope: ng.IScope, property: Common.Models.Dto.IProperty) => {
                    var activity = new Business.ActivityEditModel();

                    activity.property = new Common.Models.Business.PreviewProperty(property);
                    activity.propertyId = activity.property.id;

                    $scope['activity'] = activity;
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
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise.then((activity: Dto.IActivity) => {
                            return new Business.ActivityEditModel(activity);
                        });

                    },
                    editConfig: (activity: Business.ActivityEditModel, configService: Services.ConfigService) =>{
                        var entity = new Commands.Activity.ActivityEditCommand(activity);
                        return configService.getActivity(PageTypeEnum.Update,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            entity);
                    },
                    viewConfig: (activity: Business.ActivityEditModel, configService: Services.ConfigService) => {
                        var entity = new Commands.Activity.ActivityEditCommand(activity);
                        return configService.getActivity(PageTypeEnum.Details,
                            activity.property.propertyTypeId,
                            activity.activityTypeId,
                            entity);
                    },
                    config: (editConfig: IActivityEditConfig, viewConfig: IActivityViewConfig, activityConfigUtils: ActivityConfigUtils) =>{
                        return activityConfigUtils.merge(editConfig, viewConfig);
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
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise.then((activity: Dto.IActivity) => {
                            return new Business.ActivityViewModel(activity);
                        });
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