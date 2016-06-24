/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('attachmentPreviewCard', {
        templateUrl: 'app/common/components/attachment/previewCard/attachmentPreviewCard.html',
        controllerAs: 'vm',
        controller: 'AttachmentPreviewCardController',
        bindings: {
            entityId: '<',
            entityType: '@',
            attachment: '<'
        }
    });
}