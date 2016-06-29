/// <reference path='../../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('requirementDescriptionPreviewControl', {
        templateUrl:'app/attributes/requirement/requirementDescription/requirementDescriptionPreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                description: '<',
                config:'<'
            }
    });
}