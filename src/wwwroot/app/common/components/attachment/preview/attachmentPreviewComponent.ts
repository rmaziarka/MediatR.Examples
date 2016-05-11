/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('attachmentPreview', {
        templateUrl: 'app/common/components/attachment/preview/attachmentPreview.html',
        controllerAs: 'vm',
        controller: 'AttachmentPreviewController',
        bindings: {
            componentId: '<'
        }
    });
}