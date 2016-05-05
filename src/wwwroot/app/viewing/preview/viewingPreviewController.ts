///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Dto = Antares.Common.Models.Dto;
        import Business = Common.Models.Business;

        export class ViewingPreviewController {
            componentId: string;
            viewing: Dto.IViewing;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
            }

            clearViewingPreview = () => {
                this.viewing = new Business.Viewing();
            }

            setViewing = (viewing: Dto.IViewing) => {
                this.viewing = viewing;
            }
        }

        angular.module('app').controller('viewingPreviewController', ViewingPreviewController);
    }
}