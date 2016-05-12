/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface INumberScope extends ng.IScope {
        number: any, 
        form: INumberFormController
    }
    
    interface INumberFormController extends ng.IFormController{
        number: ng.INgModelController;
    }

    describe('input number directive decorator', () =>{
        var scope: INumberScope,
            element: ng.IAugmentedJQuery,
            numberElement: ng.INgModelController;

        beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) =>{
            $provide.decorator('inputDirective', Antares.Common.Decorators.InputNumberDirectiveDecorator.decoratorFunction);
        }));
        
        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {
            scope = <INumberScope>$rootScope.$new();
            scope.number = null;

            element = $compile(`
                <form name="form">
                    <input type="number" id='number' ng-model='number' name="number"/>
                </form>
                `)(scope);
                
           numberElement = scope.form.number;
        }));
        it('when number is formatted with dot then value should be bounded', () =>{
            numberElement.$setViewValue('1.12');

            expect(numberElement.$valid).toBeTruthy();
            expect(scope.number).toBe(1.12);
        });

        it('when number is formatted with comma then value should be bounded', () => {
            numberElement.$setViewValue('1,12');

            expect(numberElement.$valid).toBeTruthy();
            expect(scope.number).toBe(1.12);
        });

        it('when number has invalid format then value should not be bounded', () => {
            numberElement.$setViewValue("1.");

            expect(numberElement.$valid).toBeFalsy();
            expect(scope.number).toBeUndefined();
        });
    });
}