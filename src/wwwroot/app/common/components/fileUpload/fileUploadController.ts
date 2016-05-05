/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    export class FileUploadController {
        public attachmentTypes: any[];
        public file: File = null;
        public documentTypeId: string;
        private documentTypeList: Array<Dto.IEnumItem>;
        private urlResource: ng.resource.IResourceClass<Antares.Common.Models.Resources.IUrlAttachmentResource>;

        componentId: string;
        enumDocumentType: Dto.EnumTypeCode;

        constructor(
            private $scope: ng.IScope,
            private componentRegistry: Core.Service.ComponentRegistry,
            private enumService: Services.EnumService,
            private dataAccessService: Services.DataAccessService) {

            this.enumService.getEnumsPromise().then(this.onEnumsLoaded);

            this.urlResource = dataAccessService.getAzureUrlResource();
            componentRegistry.register(this, this.componentId);
        }

        private onEnumsLoaded = (result: any) => {
            this.documentTypeList = result[Antares.Common.Models.Dto.EnumTypeCode.ActivityDocumentType];
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

        uploadAttachment(activityId: number, onAttachmentUploaded: Function) {
            if (this.isDataValid()) {
                //upload to azure
                //on success ->

                // TODO check if element exists
                var documentTypeCode : string =
                    _.find(this.documentTypeList, (type: Dto.IEnumItem) => { return type.id === this.documentTypeId }).code;

                // TODO WIP stil not working, 500 returned
                var url = this.urlResource.get({
                    activityDocumentType: documentTypeCode,
                    localeIsoCode: 'en',
                    externalDocumentId: _.uniqueId(),
                    filename: this.file.name
                }).$promise.then((url: any) =>{
                     console.log('url:' + url);
                });

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