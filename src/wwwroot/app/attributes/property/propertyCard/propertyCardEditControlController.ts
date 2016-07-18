/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import CompanyContactType = Antares.Common.Models.Enums.CompanyContactType;
    import ICompanyContact = Antares.Common.Models.Dto.ICompanyContact;

    export class PropertyCardEditControlController {
        // binding
        property: Business.PreviewProperty;
        config: IPropertyCardEditControlConfig;
        onAddEdit: (obj: { property: Business.PreviewProperty}) => void;

        constructor(private $state: ng.ui.IStateService) { }

        showSelectPanel = () => {
            this.onAddEdit({ property: this.property});
        }

        navigateToProperty = (property: Business.PreviewProperty) => {
            this.$state.go('app.property-view', { id: property.id });
        }
    }

    angular.module('app').controller('propertyCardEditControlController', PropertyCardEditControlController);
};