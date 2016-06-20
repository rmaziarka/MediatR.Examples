/// <reference path="../typings/_all.d.ts" />

module Antares.Preferences {
    angular.module('app').component('preferences', {
        templateUrl: 'app/preferences/preferences.html',
        controllerAs: 'vm',
        controller: 'PreferencesController',
        bindings: {
            userData: '='
        }
    });
}
