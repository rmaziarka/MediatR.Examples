/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyEditController {
        tenancy: Business.TenancyEditModel;

        constructor(private $state: ng.ui.IStateService) {}

        configMock: Antares.Tenancy.ITenancyEditConfig = {
            term: {
                term: {
                    active: true,
                    required: true
                }
            }
        };

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
            this.$state.go('app.activity-view', {id: this.tenancy.activity.id});
        }

        save = () => {
        }

        isEditMode = (): boolean => {
            return !!this.tenancy.id;
        }
    }

    angular.module('app').controller('TenancyEditController', TenancyEditController);
}