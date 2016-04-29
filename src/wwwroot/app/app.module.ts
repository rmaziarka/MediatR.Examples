/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app', [
        'ngResource',
        'ngMessages',
        'ui.router',
        'pascalprecht.translate',
        'app.requirement',
        'ui.bootstrap',
        'fmTimepicker'
    ]);
}