/// <reference path="../../../typings/_all.d.ts" />

module Antares.FileUpload {
    angular.module('app').component('fileUpload', {
        templateUrl: 'app/common/components/fileUpload/fileUpload.html',
        controllerAs : 'fuvm',
        controller: 'FileUploadController',
        bindings: {
            componentId: '<'
        }
    });
}