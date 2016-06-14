/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import AzureBlobUploadFactory = Antares.Factories.AzureBlobUploadFactory;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    interface IAttachmentUploadCardChange {
        attachmentClear: { currentValue: any, previousValue: any }
    }

    export class AttachmentUploadCardController {
        // bindings
        entityId: string;
        entityType: Enums.EntityTypeEnum;
        enumDocumentType: Dto.EnumTypeCode;
        onUploadStarted: () => void;
        onUploadFinished: (obj: { attachment: AttachmentUploadCardModel }) => void;
        onUploadFailed: () => void;

        // controller
        public attachmentUploadForm: ng.IFormController;
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

            this.urlResource = dataAccessService.getAzureUploadUrlResource();
        }

        $onChanges(changesObj: IAttachmentUploadCardChange) {
            if (changesObj.attachmentClear && changesObj.attachmentClear.currentValue === true) {
                this.clearAttachmentForm();
            }
        }

        clearSelectedFile = () => {
            this.file = null;
            this.isFileCleared = true;
        };

        clearAttachmentForm = () => {
            this.file = null;
            this.isFileCleared = false;
            this.documentTypeId = null;

            this.attachmentUploadForm.$setPristine();
        };

        cancel = () => {
            this.eventAggregator.publish(new CloseSidePanelEvent());
        }

        uploadAttachment = () => {
            if (this.file === null) {
                this.isFileCleared = true;
                return this.reject();
            }

            this.onUploadStarted();

            var getAzureUploadUrl = this.getAzureUploadUrl();
            this.$q
                .all([
                    getAzureUploadUrl,
                    getAzureUploadUrl.then(this.uploadFile, this.reject)
                ])
                .then(
                // success
                (values: [Dto.IAzureUploadUrlContainer]) => {
                    this.onUploadFinished({
                        attachment: this.createAttachment(<string>values[0].externalDocumentId)
                    });
                },
                // error
                () => this.onUploadFailed()
                );
        };

        private getAzureUploadUrl = () => {
            return this.urlResource
                .get({
                    entityType: this.entityType,
                    documentTypeId: this.documentTypeId,
                    localeIsoCode: 'en',
                    entityReferenceId: this.entityId,
                    filename: this.file.name
                })
                .$promise;
        }

        private uploadFile = (urlContainer: Dto.IAzureUploadUrlContainer) => {
            return this.azureBlobUploadFactory
                .uploadFile(this.file, urlContainer.url);
        }

        private reject = () => {
            var uploadResult = this.$q.defer();
            uploadResult.reject();

            return uploadResult.promise;
        }

        private createAttachment = (externalDocumentId: string): AttachmentUploadCardModel => {
            var attachment = new AttachmentUploadCardModel();
            attachment.externalDocumentId = externalDocumentId;
            attachment.fileName = this.file.name;
            attachment.size = this.file.size;
            attachment.documentTypeId = this.documentTypeId;

            return attachment;
        }
    }

    angular.module('app').controller('AttachmentUploadCardController', AttachmentUploadCardController);
}