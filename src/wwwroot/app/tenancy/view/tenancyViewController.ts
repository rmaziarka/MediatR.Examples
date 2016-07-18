/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyViewController {
        // bindings
        tenancy: Business.TenancyViewModel;
        config: ITenancyViewConfig;

        constructor(private $state: ng.ui.IStateService) {
        }

        termAgreedRentSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'termAgreedRent',
            translationKey: 'TENANCY.COMMON.AGREED_RENT',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        termStartDateSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'termAgreedRent',
            translationKey: 'TENANCY.COMMON.START_DATE',
        }

        termEndDateSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'termAgreedRent',
            translationKey: 'TENANCY.COMMON.END_DATE'
        }

        navigateToActivity = () => {
            this.$state.go('app.activity-view', { id: this.tenancy.activity.id });
        }

        navigateToRequirement = () => {
            this.$state.go('app.requirement-view', { id: this.tenancy.requirement.id });
        }

        goToEdit = () => {
            this.$state.go('app.tenancy-edit', { id: this.tenancy.id });
        }

        private getTenantsNames = () => {
            return this.tenancy.tenants.map((tenant: Business.Contact) => { return tenant.getName() }).join(", ");
        }

        private getAddressTextHeader = (): string =>{
            var addressCommaSeparatedElements: string[] = [];

            if (this.tenancy.activity.property.address.propertyNumber) {
                addressCommaSeparatedElements.push(this.tenancy.activity.property.address.propertyNumber);
            }
            if (this.tenancy.activity.property.address.propertyName) {
                addressCommaSeparatedElements.push(this.tenancy.activity.property.address.propertyName);
            }
            if (this.tenancy.activity.property.address.line2) {
                addressCommaSeparatedElements.push(this.tenancy.activity.property.address.line2);
            }

            if(addressCommaSeparatedElements.length > 0){
                return addressCommaSeparatedElements.join(", ");
            }

            return null;
        }

        public getHeader = () => {
            var headerParts: string[] = [this.getTenantsNames(), this.getAddressTextHeader()];
            return headerParts.filter((part: string) => {return !!part}).join(' - ');
        };
    }

    angular.module('app').controller('TenancyViewController', TenancyViewController);
}
