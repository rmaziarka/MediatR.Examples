/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    angular.module('app').component('attachmentPreviewPanel', {
        templateUrl: 'app/common/components/attachment/previewPanel/attachmentPreviewPanel.html',
        controllerAs: 'vm',
        controller: 'AttachmentPreviewPanelController',
        bindings: {
            // base controller
            isVisible: '<',
            // controller
            entityId: '<',
            entityType: '@',
            attachment: '<'
        }
    });
}