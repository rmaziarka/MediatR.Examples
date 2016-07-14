/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('searchUser', {
        templateUrl: 'app/attributes/searchUser/searchUser.html',
        controllerAs : 'vm',
        controller: 'SearchUserController',
        bindings: {
            config: '<',
            schema: '<',
            user: '='
        }
    });
}