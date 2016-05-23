/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IFocusOnScope extends ng.IScope {
        expression: boolean
    }

    describe('Given input with focusOn directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IFocusOnScope,
            timeout: ng.ITimeoutService,
            assertValidator: TestHelpers.AssertValidators;

        var firstInput: ng.IAugmentedJQuery,
            secondInput: ng.IAugmentedJQuery;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService, $compile: ng.ICompileService) => {
            scope = <IFocusOnScope>$rootScope.$new();
            timeout = $timeout;

            // arrange
            scope.expression = false;
            element = $compile('<div><input id="firstInput" type="text" focus-on="expression"/><input id="secondInput" type="text" /></div>')(scope);
            scope.$apply();

            firstInput = element.find('input#firstInput');
            secondInput = element.find('input#secondInput');

            secondInput.focus();
        }));

        it('when expression is positive then input should be focused', () => {
            // arrange
            spyOn(firstInput[0], 'focus');

            // act
            scope.expression = true;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].focus).toHaveBeenCalled();
        });

        it('when expression is negative then input should not be focused', () => {
            // arrange
            spyOn(firstInput[0], 'focus');

            // act
            scope.expression = false;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].focus).not.toHaveBeenCalled();
        });
    });

    describe('Given element with focusOn directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IFocusOnScope,
            timeout: ng.ITimeoutService,
            assertValidator: TestHelpers.AssertValidators;

        var firstInput: ng.IAugmentedJQuery,
            secondInput: ng.IAugmentedJQuery,
            thirdInput: ng.IAugmentedJQuery;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService, $compile: ng.ICompileService) => {
            scope = <IFocusOnScope>$rootScope.$new();
            timeout = $timeout;

            // arrange
            scope.expression = false;
            element = $compile('<div><div focus-on="expression"><input id="firstInput" type="text"/><input id="secondInput" type="text" /></div><input id="thirdInput" type="text" /></div>')(scope);
            scope.$apply();

            firstInput = element.find('input#firstInput');
            secondInput = element.find('input#secondInput');
            thirdInput = element.find('input#thirdInput');

            thirdInput.focus();
        }));

        it('when expression is positive then first input within element should be focused', () => {
            // arrange
            spyOn(firstInput[0], 'focus');

            // act
            scope.expression = true;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].focus).toHaveBeenCalled();
        });

        it('when expression is negative then input should not be focused', () => {
            // arrange
            spyOn(firstInput[0], 'focus');

            // act
            scope.expression = false;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].focus).not.toHaveBeenCalled();
        });
    });
}