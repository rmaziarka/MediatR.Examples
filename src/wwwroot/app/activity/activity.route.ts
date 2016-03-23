/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider) {
        $stateProvider
            .state('app.activity-view', {
                url: '/activity/view',
                templateUrl: 'app/activity/view/activityView.html',
                controllerAs: 'vm',
                controller: 'activityViewController'
            });
    }
}