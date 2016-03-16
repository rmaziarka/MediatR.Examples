/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given contact is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators;

        var pageObjectSelectors = {
            titleSelector: 'input#title',
            firstNameSelector: 'input#first-name',
            surnameSelector: 'input#surname'
        };

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            element = $compile('<contact-add></contact-add>')(scope);
            scope.$apply();

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        it('when title value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.titleSelector);
        });

        it('when title value is present then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('Miss', true, pageObjectSelectors.titleSelector);
        });
        
        it('when title value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.titleSelector);
        });

        it('when title value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.titleSelector);
        });

        /////////

        it('when first name value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value is present then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('Name', true, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.firstNameSelector);
        });

        /////////

        it('when surname value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.surnameSelector);
        });

        it('when surname value is present then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('Name', true, pageObjectSelectors.surnameSelector);
        });

        it('when surname value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.surnameSelector);
        });

        it('when surname value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.surnameSelector);
        });
    });
}