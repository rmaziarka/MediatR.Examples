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
        
        constructor(private $scope: ng.IScope,
            private componentRegistry: Core.Service.ComponentRegistry) {

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

        saveAttachment() {
            if(this.isDataValid()){
                var attachment = new Business.Attachment();

                attachment.fileName = this.file.name;
                attachment.size = this.file.size;
                attachment.fileTypeId = this.documentTypeId;
                
                console.log('Attachment:SAVED');
            }
        };
    }

    angular.module('app').controller('FileUploadController', FileUploadController);
}