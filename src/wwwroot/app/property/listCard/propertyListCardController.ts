///<reference path="../../typings/_all.d.ts"/>

module Antares.Property.ListCard {
    export module Component {
        import Business = Common.Models.Business;
        import Dto = Common.Models.Dto;

        export class PropertyListCardController {

            //bindings
            properties: Business.PreviewPropertyWithSelection[];
            isLoading: boolean;
            allowMultipleSelect: boolean;
            onSave: (obj: { properties: Dto.IPreviewProperty[]}) => void;
            onCancel: () => void;

            constructor(private $state: ng.ui.IStateService, private $window: ng.IWindowService) { }

            close = () => {
                this.onCancel();
            }

            cardSelected = (property: Business.PreviewPropertyWithSelection, selected: boolean) => {
                if (!this.allowMultipleSelect && selected) {
                    this.properties.forEach((p: Business.PreviewPropertyWithSelection) => {
                        p.selected = false;
                    });
                }

                var selectedContact = this.properties.filter((p: Business.PreviewPropertyWithSelection) => {
                    return p.id === property.id;
                })[0];

                selectedContact.selected = selected;
            }

            navigateToProperty = (property: Dto.IProperty) => {
                var url = this.$state.href('app.property-view', { id: property.id }, { absolute: true });
                this.$window.open(url, '_blank')
            }

            save = () => {
                var selectedProperties = this.properties
                    .filter((c: any) => c.selected);

                this.onSave({ properties : selectedProperties });
            }
        }

        angular.module('app').controller('propertyListCardController', PropertyListCardController);

    }
}