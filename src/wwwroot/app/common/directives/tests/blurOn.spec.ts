/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IBlurOnScope extends ng.IScope {
        expression: boolean
    }

    describe('Given input with blurOn directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IBlurOnScope,
            timeout: ng.ITimeoutService,
            assertValidator: TestHelpers.AssertValidators;

        var firstInput: ng.IAugmentedJQuery,
            secondInput: ng.IAugmentedJQuery;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService, $compile: ng.ICompileService) => {
            scope = <IBlurOnScope>$rootScope.$new();
            timeout = $timeout;

            // arrange
            scope.expression = false;
            element = $compile('<div><input id="firstInput" type="text" blur-on="expression"/><input id="secondInput" type="text" /></div>')(scope);
            scope.$apply();

            firstInput = element.find('input#firstInput');
            secondInput = element.find('input#secondInput');

            firstInput.focus();
        }));

        it('when expression is positive then input should be blured', () => {
            // arrange
            spyOn(firstInput[0], 'blur');

            // act
            scope.expression = true;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].blur).toHaveBeenCalled();
        });

        it('when expression is negative then input should not be blured', () => {
            // arrange
            spyOn(firstInput[0], 'blur');

            // act
            scope.expression = false;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].blur).not.toHaveBeenCalled();
        });
    });

    describe('Given element with focusOn directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IBlurOnScope,
            timeout: ng.ITimeoutService,
            assertValidator: TestHelpers.AssertValidators;

        var firstInput: ng.IAugmentedJQuery,
            secondInput: ng.IAugmentedJQuery,
            thirdInput: ng.IAugmentedJQuery;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService, $compile: ng.ICompileService) => {
            scope = <IBlurOnScope>$rootScope.$new();
            timeout = $timeout;

            // arrange
            scope.expression = false;
            element = $compile('<div><div blur-on="expression"><input id="firstInput" type="text"/><input id="secondInput" type="text" /></div><input id="thirdInput" type="text" /></div>')(scope);
            scope.$apply();

            firstInput = element.find('input#firstInput');
            secondInput = element.find('input#secondInput');
            thirdInput = element.find('input#thirdInput');

            firstInput.focus();
        }));

        it('when expression is positive then first input within element should be blured', () => {
            // arrange
            spyOn(firstInput[0], 'blur');

            // act
            scope.expression = true;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].blur).toHaveBeenCalled();
        });

        it('when expression is negative then input should not be blured', () => {
            // arrange
            spyOn(firstInput[0], 'blur');

            // act
            scope.expression = false;
            scope.$apply();
            timeout.flush();

            // assert
            expect(firstInput[0].blur).not.toHaveBeenCalled();
        });
    });
}