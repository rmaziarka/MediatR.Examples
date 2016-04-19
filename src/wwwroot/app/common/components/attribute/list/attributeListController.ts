/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AttributeListController {
        private componentId: string;
        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        public property: Business.Property;
        public userData: Dto.IUserData;
        private attributes: Business.Attribute[];

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService) {

            componentRegistry.register(this, this.componentId);
            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadAttributes();
        }
        
        loadAttributes = () => {
            if (this.property.propertyTypeId) {
                this.attributes = null;
                this.propertyResource
                    .getAttributes({
                        countryCode: this.userData.country, propertyTypeId: this.property.propertyTypeId
                    }, null)
                    .$promise
                    .then((attributes: any) => {
                        this.attributes = attributes.attributes.map(function (item: Dto.IAttribute) {
                            return new Business.Attribute(item);
                        });
                    });
            }
        }

        clearAttributes = () => {
            this.attributes = null;
        }
    }

    angular.module('app').controller('attributeListController', AttributeListController);
}