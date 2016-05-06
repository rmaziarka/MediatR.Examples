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

        uploadAttachment(activityId: string, onAttachmentUploaded: Function) {
            if (this.isDataValid()) {
                this.urlResource.get({
                        documentTypeId : this.documentTypeId,
                        localeIsoCode: 'en',
                        entityReferenceId: activityId,
                        filename : this.file.name
                    })
                    .$promise
                    .then((urlContainer: Antares.Common.Models.Dto.IAzureUploadUrlContainer) => {
                        var url: string = urlContainer.url;

                        this.azureBlobUploadFactory.uploadFile(this.file, url)
                            .then(() =>{
                                var attachment = new Common.Models.Business.Attachment();

                                attachment.externalDocumentId = urlContainer.externalDocumentId;
                                attachment.fileName = this.file.name;
                                attachment.size = this.file.size;
                                attachment.documentTypeId = this.documentTypeId;

                                onAttachmentUploaded(attachment);
                            });
                    }, () => {
                        console.error('Error sending file to azure storage.');
                    });
            }
        };
    }

    angular.module('app').controller('FileUploadController', FileUploadController);
}