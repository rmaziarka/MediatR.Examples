/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    export class FileUploadController {
        public attachmentTypes: any[];
        public file: File = null;
        public documentTypeId: string;

        componentId: string;
        enumDocumentType: Dto.EnumTypeCode;

        constructor(
            private $scope: ng.IScope,
            private componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

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

	    uploadAttachment(onAttachmentUploaded: Function) {
            if (this.isDataValid()) {
                //upload to azure
                //on success ->

                var attachment = new Antares.Common.Models.Business.Attachment();

                attachment.externalDocumentId = '664940CC-8212-E611-8271-8CDCD42E5436';
                attachment.fileName = this.file.name;
                attachment.size = this.file.size;
                attachment.documentTypeId = this.documentTypeId;

	            onAttachmentUploaded(attachment);
            }
        };
    }

    angular.module('app').controller('FileUploadController', FileUploadController);
}