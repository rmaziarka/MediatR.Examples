/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;

    export class AttachmentPreviewController {
        public componentId: string;
        public attachment: Common.Models.Business.Attachment = <Common.Models.Business.Attachment>{};
        public attachmentUrl: string;

        private urlResource: ng.resource.IResourceClass<Common.Models.Resources.IAzureDownloadUrlResource>;

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            this.urlResource = dataAccessService.getAzureDownloadUrlResource();

            componentRegistry.register(this, this.componentId);
        }

        setAttachment = (attachment: Common.Models.Business.Attachment, activityId: string) => {
            this.attachmentUrl = '';
            this.attachment = attachment;

            this.urlResource.get({
                    documentTypeId : this.attachment.documentTypeId,
                    localeIsoCode: 'en',
                    externalDocumentId: this.attachment.externalDocumentId,
                    entityReferenceId: activityId,
                    filename : this.attachment.fileName
                }).$promise
                .then((url: Antares.Common.Models.Dto.IAzureDownloadUrlContainer) =>{
                    this.attachmentUrl = url.url;
                }, () =>{
                    console.error('Error sending file to azure storage.');
                });
        }
    }

    angular.module('app').controller('AttachmentPreviewController', AttachmentPreviewController);
}