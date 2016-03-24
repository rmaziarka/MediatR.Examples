/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component{
    angular.module('app').component('requirementAddressFormView',{
        controller:'addressFormViewController',
        controllerAs:'vm',
        templateUrl:'app/requirement/view/addressView/requirementAddressFormView.html',
        bindings:{
            address:'<'
        }
    });
}