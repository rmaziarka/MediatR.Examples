/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    var app: ng.IModule = angular.module('app.requirement');

    app.config(initRoute);

    function initRoute($stateProvider) {
        $stateProvider
            .state('app.requirement-add', {
                url: '/requirement/add',
                templateUrl: 'app/requirement/add/requirementAdd.html',
                controllerAs: 'vm',
                controller: 'requirementAddController'
            });
    }
}