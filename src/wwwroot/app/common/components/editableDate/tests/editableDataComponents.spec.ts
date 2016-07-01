/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import EditableDateController = Antares.Common.Component.EditableDateController

    describe('Given editable data component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            controller: EditableDateController,
            callDateToTest: any,
            pageObjectSelectors = {
                timeElement: 'time',
                pencilElement: '.fa-pencil',
                dateEditInput: 'input#next-call-1',
                calendarElement: '.fa-calendar'
            };

        describe('not in edit mode', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                compile = $compile;
                callDateToTest = moment().format('DD-MM-YYYY');

                var mockedEditableDateComponent = '<editable-date selected-date="callDate" is-required="true" can-be-edited="true" on-save=""></editable-date>';
                scope['callDate'] = callDateToTest;

                element = compile(mockedEditableDateComponent)(scope);

                controller = element.controller('editableDate');

                scope.$apply();
            }));

            it('then date is displayed correctly', () => {
                var timeText = element.find(pageObjectSelectors.timeElement).text();

                expect(timeText).toEqual(callDateToTest);
            });

            it('then pencil icon is displayed', () => {
                var pencilIcon = element.find(pageObjectSelectors.pencilElement);

                expect(pencilIcon.length).toBe(1);
            });

            it('then no input field is displayed', () => {
                var inputField = element.find(pageObjectSelectors.dateEditInput);

                expect(inputField.length).toBe(0);
            });

            it('then no calendar field is displayed', () => {
                var inputField = element.find(pageObjectSelectors.calendarElement);

                expect(inputField.length).toBe(0);
            });
        });

        describe('in edit mode', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                compile = $compile;

                callDateToTest = moment().format('DD-MM-YYYY');

                var mockedEditableDateComponent = '<editable-date selected-date="callDate" is-required="true" ng-model="date" can-be-edited="true" on-save="fakeMethod()"></editable-date>';
                scope['callDate'] = callDateToTest;
                scope['date'] = callDateToTest;

                element = compile(mockedEditableDateComponent)(scope);

                scope.$apply();

                controller = element.controller('editableDate');

                controller.openEditMode();

                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('then pencil icon is not displayed', () => {
                var pencilIcon = element.find(pageObjectSelectors.pencilElement);

                expect(pencilIcon.length).toBe(0);
            });

            it('then input field is displayed', () => {
                var editInputField = element.find(pageObjectSelectors.dateEditInput);

                expect(editInputField.length).toBe(1);
            });

            it('then calendar field is displayed', () => {
                var calendarField = element.find(pageObjectSelectors.calendarElement);

                expect(calendarField.length).toBe(1);
            });

            it('then submit with correct date format should not return validation error', () => {
                assertValidator.assertRequiredValidator(callDateToTest, true, pageObjectSelectors.dateEditInput);
            });

            it('then submit with empty date field should return validation error', () => {
                assertValidator.assertRequiredValidator('', false, pageObjectSelectors.dateEditInput);
            });

            it('then submit with invalid date format should return validation error', () => {
                assertValidator.assertDateFormatValidator('invalid_format', false, pageObjectSelectors.dateEditInput);
            });
        });
    });
}