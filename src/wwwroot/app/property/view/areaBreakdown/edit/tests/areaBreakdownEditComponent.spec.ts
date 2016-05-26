/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;

    describe('Given areaBreakdown edit component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelectors = {
            firstNameInput: '.area-details:first input#name',
            firstSizeInput: '.area-details:first input#size',
            addAreaGroup: '.area-details'
        }

        var controller: Property.View.AreaBreakdown.AreaBreakdownEditController;
        var areaToUpdateMock = TestHelpers.PropertyAreaBreakdownGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;

            scope = $rootScope.$new();
            element = $compile('<area-breakdown-edit></area-breakdown-edit>')(scope);

            scope.$apply();

            controller = element.controller('areaBreakdownEdit');
            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        type TestCaseForValidator = [string | number, boolean];

        runDescribe('when filling name')
            .data<TestCaseForValidator>([
                ['', false],
                [null, false],
                ['text', true]])
            .dataIt((data: TestCaseForValidator) =>
                `and value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForValidator) => {
                // arrange / act / assert
                controller.editPropertyAreaBreakdown(areaToUpdateMock);
                scope.$apply();

                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.firstNameInput);
            });

        runDescribe('when filling name')
            .data<TestCaseForValidator>([
                [129, false],
                [128, true]])
            .dataIt((data: TestCaseForValidator) =>
                `and length is "${data[0]}" then max length message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForValidator) => {
                // arrange / act / assert
                controller.editPropertyAreaBreakdown(areaToUpdateMock);
                scope.$apply();

                assertValidator.assertMaxLengthValidator(<number>data[0], data[1], pageObjectSelectors.firstNameInput);
            });

        runDescribe('when filling size')
            .data<TestCaseForValidator>([
                ['', false],
                [null, false],
                ['1', true]])
            .dataIt((data: TestCaseForValidator) =>
                `and value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForValidator) => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.firstSizeInput);
            });

        runDescribe('when filling size')
            .data<TestCaseForValidator>([
                [0, false],
                [-1, false],
                [1, true]])
            .dataIt((data: TestCaseForValidator) =>
                `and value is "${data[0]}" then greater then message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForValidator) => {
                // arrange / act / assert
                assertValidator.assertNumberGreaterThenValidator(<number>data[0], data[1], pageObjectSelectors.firstSizeInput);
            });
    });
}