/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyEditController {
        // bindings
        tenancy: Business.TenancyEditModel;
        config: Antares.Tenancy.ITenancyEditConfig;

        constructor(private $state: ng.ui.IStateService, private tenancyService: Antares.Services.TenancyService) {
        }

        $onInit = () => {
            this.setDefaultContacts();
        }

        termDateSchema: Antares.Attributes.IDateRangeControlSchema = {
            dateFromControlId: 'termDateFrom',
            dateToControlId: 'termDateTo',
            dateFromTranslationKey: 'TENANCY.COMMON.START_DATE',
            dateToTranslationKey: 'TENANCY.COMMON.END_DATE',
            formName: 'termDateForm',
            fieldName: 'term'
        };

        termAgreedRentSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'termAgreedRent',
            translationKey: 'TENANCY.COMMON.AGREED_RENT',
            formName: 'termAgreedRentsForm',
            fieldName: 'term',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        navigateToActivity = (ativity: Business.ActivityPreviewModel) => {
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Business.RequirementPreviewModel) => {
            this.$state.go('app.requirement-view', { id: requirement.id });
        }

        cancel() {
            this.$state.go('app.activity-view', { id: this.tenancy.activity.id });
        }

        save = () => {
            if (this.isEditMode()) {
                this.tenancyService.updateTenancy(new Antares.Common.Models.Commands.Tenancy.TenancyEditCommand(this.tenancy)).then((dto: Antares.Common.Models.Dto.ITenancy) => {
                    this.navigateToTenancyView(dto.id);
                });
            }
            else {
                this.tenancyService.addTenancy(new Antares.Common.Models.Commands.Tenancy.TenancyAddCommand(this.tenancy)).then((dto: Antares.Common.Models.Dto.ITenancy) => {
                    this.navigateToTenancyView(dto.id);
                });
            }
        }

        isEditMode = (): boolean => {
            return !!this.tenancy.id;
        }

        isAddMode = (): boolean => {
            return !this.isEditMode();
        }

        private navigateToTenancyView = (tenancyId: string) => {
            this.$state.go('app.tenancy-view', { id: tenancyId });
        }

        private setDefaultContacts = () => {
            if (this.isEditMode()) {
                return;
            }

            this.tenancy.landlords = this.tenancy.activity.landlords;
            this.tenancy.tenants = this.tenancy.requirement.contacts;
        }
    }

    angular.module('app').controller('TenancyEditController', TenancyEditController);
}