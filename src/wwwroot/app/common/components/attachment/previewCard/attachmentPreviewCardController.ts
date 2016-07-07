/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    interface IAttachmentPreviewCardChange {
        attachment: { currentValue: any, previousValue: any }
    }

    export class AttachmentPreviewCardController {
        // bindings
        entityId: string;
        entityType: Enums.EntityTypeEnum;
        attachment: Common.Models.Business.Attachment = <Common.Models.Business.Attachment>{};

        // controller
        public attachmentUrl: string;

        private urlResource: ng.resource.IResourceClass<Common.Models.Resources.IAzureDownloadUrlResource>;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.urlResource = dataAccessService.getAzureDownloadUrlResource();
        }

        $onChanges(changesObj: IAttachmentPreviewCardChange) {
            this.attachmentUrl = '';

            if (changesObj.attachment && changesObj.attachment.currentValue.externalDocumentId) {
                this.setupAttachmentUrl();
            }
        }

        private setupAttachmentUrl = () => {
            this.urlResource.get({
                    entityType: this.entityType,
                    documentTypeId : this.attachment.documentTypeId,
                    localeIsoCode: 'en',
                    externalDocumentId: this.attachment.externalDocumentId,
                    entityReferenceId: this.entityId,
                    filename : this.attachment.fileName
                }).$promise
                .then((url: Antares.Common.Models.Dto.IAzureDownloadUrlContainer) =>{
                    this.attachmentUrl = url.url;
                }, () =>{
                    console.error('Error gathering url for file in azure storage.');
                });
        }
    }

    angular.module('app').controller('AttachmentPreviewCardController', AttachmentPreviewCardController);
}