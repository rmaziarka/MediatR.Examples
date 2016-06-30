/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    angular.module('app').component('attachmentsManager', {
        templateUrl: 'app/common/components/attachment/manager/attachmentsManager.html',
        controllerAs: 'vm',
        controller: 'AttachmentsManagerController',
        bindings: {
            data: '<',
            onSaveAttachmentForEntity: '&',
            title: '@',
            filesNumberLimit: '<'
        }
    });
}