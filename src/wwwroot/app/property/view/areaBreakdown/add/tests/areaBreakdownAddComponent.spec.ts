/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;

    describe('Given areaBreakdownAdd component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelectors = {
            firstNameInput: '.area-details:first input#name',
            firstSizeInput: '.area-details:first input#size',
            addAreaGroup: '.area-details'
        }

        var controller: Property.View.AreaBreakdown.AreaBreakdownAddController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;

            scope = $rootScope.$new();
            element = $compile('<area-breakdown-add></area-breakdown-add>')(scope);

            scope.$apply();

            controller = element.controller('areaBreakdownAdd');
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
                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.firstNameInput);
            });

        it('then it should validate if name field has less than 129 characters', () => {
            // act
            controller.addNewArea();
            scope.$apply();

            // assert
            assertValidator.assertMaxLengthValidator(129, false, pageObjectSelectors.firstNameInput);
            assertValidator.assertMaxLengthValidator(128, true, pageObjectSelectors.firstNameInput);
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

        it('then it should generate correct number of area form groups', () => {
            // arrange & act
            controller.addNewArea();
            scope.$apply();
            var areasCount = controller.areas.length;

            // assert
            var areasGroups = element.find(pageObjectSelectors.addAreaGroup);
            expect(areasGroups.length).toBe(areasCount);
        });
    });
}