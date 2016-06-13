/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    angular.module('app').component('attachmentUploadPanel', {
        templateUrl: 'app/common/components/attachment/uploadPanel/attachmentUploadPanel.html',
        controllerAs: 'vm',
        controller: 'AttachmentUploadPanelController',
        bindings: {
            isVisible: '<',
            saveAttachmentForEntity: '<',
            enumDocumentType: '@',
            entityType: '@',
            entityId: '<'
        }
    });
}