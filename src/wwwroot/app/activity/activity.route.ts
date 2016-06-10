/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {

    var app: ng.IModule = angular.module('app');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.activity-view', {
                url: '/activity/view/:id',
                template: "<activity-view activity='activity'></activity-view>",
                controller: "ActivityRouteController",
                resolve: {
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['id'];
                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
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
                    }
                }
            });
    }
}