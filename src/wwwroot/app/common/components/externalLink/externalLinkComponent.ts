/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component{
    angular.module('app').component('externalLink', {
        
        controller:'ExternalLinkController',
        controllerAs:'vm',
        templateUrl:'app/common/components/externalLink/externalLink.html',
        bindings:{
            url: '=',
            showText: '='
        }
    });
}