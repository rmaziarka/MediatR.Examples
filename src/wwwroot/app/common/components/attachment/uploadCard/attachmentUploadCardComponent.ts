/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    angular.module('app').component('attachmentUploadCard', {
        templateUrl: 'app/common/components/attachment/uploadCard/attachmentUploadCard.html',
        controllerAs: 'vm',
        controller: 'AttachmentUploadCardController',
        bindings: {
            entityId: '<',
            entityType: '@',
            enumDocumentType: '@',
            onUploadStarted: '&',
            onUploadFinished: '&',
            onUploadFailed: '&',
            attachmentClear: '<'
        }
    });
}