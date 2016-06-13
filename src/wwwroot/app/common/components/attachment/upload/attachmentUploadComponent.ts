/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('attachmentUpload', {
        templateUrl: 'app/common/components/attachment/upload/attachmentUpload.html',
        controllerAs : 'auvm',
        controller: 'AttachmentUploadController',
        bindings: {
            componentId: '<',
            enumDocumentType: '@',
            entityType: '@'
        }
    });
}