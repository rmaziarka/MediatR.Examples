/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import AzureBlobUploadFactory = Antares.Factories.AzureBlobUploadFactory;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class AttachmentUploadCardController {
        // bindings
        entityType: Enums.EntityTypeEnum;
        enumDocumentType: Dto.EnumTypeCode;
        entityId: string;
        onUpload: (obj: { attachment: AttachmentUploadCardModel }) => void;

        // controller
        public attachmentTypes: any[];
        public file: File = null;
        public isFileCleared: boolean = false;
        public documentTypeId: string;

        private urlResource: ng.resource.IResourceClass<Common.Models.Resources.IAzureUploadUrlResource>;

        constructor(
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private azureBlobUploadFactory: AzureBlobUploadFactory,
            private dataAccessService: Services.DataAccessService,
            private eventAggregator: Antares.Core.EventAggregator) {

            this.urlResource = dataAccessService.getAzureUploadUrlResource(this.entityType);
        }

        isDataValid = (): boolean => {
            var form = this.$scope["attachmentUploadForm"];
            form.$setSubmitted();
            return form.$valid;
        }

        clearSelectedFile = () => {
            this.file = null;
            this.isFileCleared = true;
        };

        clearAttachmentForm = () => {
            this.file = null;
            this.isFileCleared = false;
            this.documentTypeId = null;

            var form = this.$scope["attachmentUploadForm"];
            form.$setPristine();
        };

        cancel = () => {
            this.eventAggregator.publish(new CloseSidePanelEvent());
        }

        getAzureUploadUrl = () => {
            return this.urlResource
                .get({
                    documentTypeId: this.documentTypeId,
                    localeIsoCode: 'en',
                    entityReferenceId: this.entityId,
                    filename: this.file.name
                })
                .$promise;
        }

        uploadFile = (urlContainer: Dto.IAzureUploadUrlContainer) => {
            return this.azureBlobUploadFactory
                .uploadFile(this.file, urlContainer.url);
        }

        reject = () => {
            var uploadResult = this.$q.defer();
            uploadResult.reject();

            return uploadResult.promise;
        }

        createAttachment = (externalDocumentId: string): AttachmentUploadCardModel => {
            var attachment = new AttachmentUploadCardModel();
            attachment.externalDocumentId = externalDocumentId;
            attachment.fileName = this.file.name;
            attachment.size = this.file.size;
            attachment.documentTypeId = this.documentTypeId;

            return attachment;
        }

        uploadAttachment = () => {
            if (this.file === null) {
                this.isFileCleared = true;
                return this.reject();
            }

            var getAzureUploadUrl = this.getAzureUploadUrl();
            this.$q
                .all([
                    getAzureUploadUrl,
                    getAzureUploadUrl.then(this.uploadFile, this.reject)
                ])
                .then((values: [Dto.IAzureUploadUrlContainer]) => {
                    this.onUpload({
                        attachment: this.createAttachment(<string>values[0].externalDocumentId)
                    });
                }
            );
        };
    }

    angular.module('app').controller('AttachmentUploadCardController', AttachmentUploadCardController);
}