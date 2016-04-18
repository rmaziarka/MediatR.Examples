/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IKfMaxDateScope extends ng.IScope {
        number: any
    }

    describe('Given input with number type', () =>{
        var scope: IKfMaxDateScope,
            element: ng.IAugmentedJQuery,
            numberAdapter: TestHelpers.InputValidationAdapter;

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {
            scope = <IKfMaxDateScope>$rootScope.$new();
            scope.number = null;

            // input should be text type for test purpose - when number then jquery val() method does not put number value formatted with comma
            element = $compile(`
                <div>
                    <input type="text" ddp-decimal id='number' ng-model="number"/>
                </div>
                `)(scope);
            scope.$apply();

            var numberEl: ng.IAugmentedJQuery = element.find('#number');
            numberAdapter = new TestHelpers.InputValidationAdapter(numberEl, null, scope);
        }));

        it('when number is formatted with dot then value should be bounded', () =>{
            numberAdapter.writeValue('1.12');

            expect(numberAdapter.isInputValid()).toBeTruthy();
            expect(scope.number).toBe(1.12);
        });

        it('when number is formatted with comma then value should be bounded', () => {
            numberAdapter.writeValue("1,12");

            expect(numberAdapter.isInputValid()).toBeTruthy();
            expect(scope.number).toBe(1.12);
        });

        it('when number has invalid format then value should not be bounded', () => {
            numberAdapter.writeValue("1.");

            expect(numberAdapter.isInputValid()).toBeFalsy();
            expect(scope.number).toBeUndefined();
        });
    });
}