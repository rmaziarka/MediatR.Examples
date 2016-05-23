/// <reference path="typings/_all.d.ts" />

module Antares {
    angular.module('app', [
        'ngResource',
        'ngMessages',
        'ui.router',
        'pascalprecht.translate',
        'app.requirement',
        'ui.bootstrap',
        'fmTimepicker',
        'ui.bootstrap',
        'ngFileUpload',
        'as.sortable',
        'angular-growl'
    ]);
}