/// <reference path="../../../typings/_all.d.ts" />


module Antares.Common.Component {

    angular.module('app').component('sidePanel', {
         transclude:true,
         template:"<div class='sidePanel'> <div ng-transclude> </div> </div>"
    });
}