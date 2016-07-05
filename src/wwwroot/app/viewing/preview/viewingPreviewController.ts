///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;

        export class ViewingPreviewController {
            // binding
            viewing: Business.Viewing;            
        }

        angular.module('app').controller('viewingPreviewController', ViewingPreviewController);
    }
}