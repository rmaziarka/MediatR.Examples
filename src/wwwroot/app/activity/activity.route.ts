/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    var app: ng.IModule = angular.module('app');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.activity-view', {
                url: '/activity/view/:id',
                template: "<activity-view activity='activity'></activity-view>",
                controller: ($scope: ng.IScope, activity: Dto.IActivity) => {
                    var activityViewModel = new Business.Activity(<Dto.IActivity>activity);

                    $scope['activity'] = activityViewModel;
                },
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    }
                }
            })
            .state('app.activity-edit', {
                url: '/activity/edit/:id',
                template: "<activity-edit activity='activity'></activity-edit>",
                controller: ($scope: ng.IScope, activity: Dto.IActivity) => {
                    var activityViewModel = new Business.Activity(<Dto.IActivity>activity);

                    $scope['activity'] = activityViewModel;
                },
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    }
                }
            })
    }
}