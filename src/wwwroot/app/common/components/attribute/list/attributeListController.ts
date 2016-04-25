/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AttributeListController {
        private componentId: string;
        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private property: Business.Property;
        private userData: Dto.IUserData;
        private attributes: Business.Attribute[];
        private mode: string;
        
        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService) {

            componentRegistry.register(this, this.componentId);
            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadAttributes();
        }

        loadAttributes = () =>{
            var countryId = this.property.address.countryId;
            if (this.property.propertyTypeId && countryId) {
                this.attributes = null;
                this.propertyResource
                    .getAttributes({
                        countryId: countryId, propertyTypeId: this.property.propertyTypeId
                    }, null)
                    .$promise
                    .then((attributes: any) =>{
                        this.attributes = attributes.attributes.map((item: Dto.IAttribute) => new Business.Attribute(item));
                    });
            }
        }

        clearAttributes = () => {
            this.attributes = null;
        }

        clearHiddenAttributesFromProperty = () => {
            for (var attributeValue in this.property.attributeValues) {
                if (this.property.attributeValues.hasOwnProperty(attributeValue)) {
                    if (attributeValue != 'id') {
                        var propertyAttributeIsHidden = !this.attributes ? true : this.attributes.filter((item) => { return item.min === attributeValue || item.max === attributeValue }).length === 0;
                        if (propertyAttributeIsHidden) {
                            this.property.attributeValues[attributeValue] = null;
                        }
                    }
                }
            }
        }
    }

    angular.module('app').controller('attributeListController', AttributeListController);
}