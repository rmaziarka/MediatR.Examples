/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app', [
        'ngResource',
        'ui.router',
        'app.frontoffice',
        'app.backoffice'
    ]);
}