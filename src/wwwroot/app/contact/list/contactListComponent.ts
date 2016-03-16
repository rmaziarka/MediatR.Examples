/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('contactList', {
        controller : 'contactListController',
        controllerAs : 'vm',
        templateUrl : 'app/contact/list/contactList.html',
        transclude : true,
        bindings : {
            componentId : '<'
        }
    });
}