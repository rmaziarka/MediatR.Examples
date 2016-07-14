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

        getTenantsNames = () => {
            return this.tenancy.tenants.map((tenant: Business.Contact) => { return tenant.getName() }).join(", ");
        }
    }

    angular.module('app').controller('TenancyViewController', TenancyViewController);
}
