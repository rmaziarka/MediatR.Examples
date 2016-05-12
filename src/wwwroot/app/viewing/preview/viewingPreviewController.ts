///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;

        export class ViewingPreviewController {
            componentId: string;
            viewing: Business.Viewing;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry) {
                componentRegistry.register(this, this.componentId);
            }

            clearViewingPreview = () => {
                this.viewing = new Business.Viewing();
            }

            getViewing = (): Business.Viewing => {
                return this.viewing;
            }

            setViewing = (viewing: Business.Viewing) =>{
                this.viewing = viewing;
            }
        }

        angular.module('app').controller('viewingPreviewController', ViewingPreviewController);
    }
}