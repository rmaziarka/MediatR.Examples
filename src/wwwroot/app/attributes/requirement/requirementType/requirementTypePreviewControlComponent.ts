/// <reference path='../../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('requirementTypePreviewControl', {
        templateUrl:'app/attributes/requirement/requirementType/requirementTypePreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                typeId: '<',
                config:'<'
            }
    });
}