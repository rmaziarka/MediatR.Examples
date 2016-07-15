/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('searchUserControl', {
        templateUrl: 'app/attributes/searchUser/searchUserControl.html',
        controllerAs : 'vm',
        controller: 'SearchUserControlController',
        bindings: {
            config: '<',
            schema: '<',
            user: '=',
            onUserChanged: '&'
        }
    });
}