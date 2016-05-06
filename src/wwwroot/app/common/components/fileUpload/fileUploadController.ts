/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import AzureBlobUploadFactory = Antares.Factories.AzureBlobUploadFactory;
    import Dto = Common.Models.Dto;

    export class FileUploadController {
        public attachmentTypes: any[];
        public file: File = null;
        public documentTypeId: string;
        private urlResource: ng.resource.IResourceClass<Common.Models.Resources.IAzureUploadUrlResource>;

        componentId: string;
        enumDocumentType: Dto.EnumTypeCode;

        constructor(
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private componentRegistry: Core.Service.ComponentRegistry,
            private azureBlobUploadFactory: AzureBlobUploadFactory,
            private dataAccessService: Services.DataAccessService) {

            this.urlResource = dataAccessService.getAzureUploadUrlResource();

            componentRegistry.register(this, this.componentId);
        }

        isDataValid = (): boolean => {
            var form = this.$scope["fileUploadForm"];
            form.$setSubmitted();
            return form.$valid;
        }

        clearSelectedFile = () => {
            this.file = null;
        };

        clearAttachmentForm = () => {
            this.clearSelectedFile();
            this.documentTypeId = null;

            var form = this.$scope["fileUploadForm"];
            form.$setPristine();
        };

        uploadAttachment(entityReferenceId: string) {
            if (!this.isDataValid()) {
                var uploadResult = this.$q.defer();
                uploadResult.reject();
                return uploadResult.promise;
            }

            return this.urlResource.get({
                documentTypeId: this.documentTypeId,
                localeIsoCode: 'en',
                entityReferenceId: entityReferenceId,
                filename: this.file.name
            })
                .$promise
                .then((urlContainer: Common.Models.Dto.IAzureUploadUrlContainer) => {
                    return this.azureBlobUploadFactory
                        .uploadFile(this.file, <string>urlContainer.url)
                        .then(() => {
                             return urlContainer.externalDocumentId;
                        });
                }, () => {
                    console.error('Error sending file to azure storage.');

                    var uploadResult = this.$q.defer();
                    uploadResult.reject();
                    return uploadResult.promise;
                })
                .then((externalDocumentId) => {
                    var attachment = new Common.Models.Business.Attachment();

                    attachment.externalDocumentId = externalDocumentId;
                    attachment.fileName = this.file.name;
                    attachment.size = this.file.size;
                    attachment.documentTypeId = this.documentTypeId;

                    return attachment;
                });
        };
    }

    angular.module('app').controller('FileUploadController', FileUploadController);
}