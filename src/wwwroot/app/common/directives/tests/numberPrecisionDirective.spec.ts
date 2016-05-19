/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseForNumberPrecisionValidator = [number, number, boolean]; // [precision, value, valid]

    interface INumberPrecisionDirectiveScope extends ng.IScope {
        number: number;
        numberPrecision: number;
        form: any;
    }

    describe('Given input is valid with number-precision directive', () => {

        var element: ng.IAugmentedJQuery,
            scope: INumberPrecisionDirectiveScope;
            
        beforeEach(inject(($rootScope: ng.IRootScopeService, $compile: ng.ICompileService) => {

            scope = <INumberPrecisionDirectiveScope>$rootScope.$new();

            scope.number = null;
            scope.numberPrecision = null;
            scope.$apply();

            element = $compile(`
                <form name=\"form\">
                    <div>
                        <input type="number" id="number" name="number" ng-model="number" number-precision="numberPrecision" />
                    </div>
                </form>
                `)(scope);
            scope.$apply();
        }));

        runDescribe('when')
            .data<TestCaseForNumberPrecisionValidator>([
                [0, 0, true],
                [0, 0.1, false],
                [0, 0.01, false],
                [0, 0.001, false],
                [1, 0, true],
                [1, 0.1, true],
                [1, 0.01, false],
                [1, 0.001, false],
                [2, 0, true],
                [2, 0.1, true],
                [2, 0.01, true],
                [2, 0.001, false],
                [-1, 0, true],
                [-1, 0.1, false]
            ])
            .dataIt((data: TestCaseForNumberPrecisionValidator) =>
                ` precision=${data[0]} and number=${data[1]} then input should be ${data[2] ? 'valid' : 'invalid'}`)
            .run((data: TestCaseForNumberPrecisionValidator) =>{
                // arrange / act / assert
                scope.numberPrecision = data[0];
                scope.$apply();

                scope.form.number.$setViewValue(data[1]);
                expect(scope.form.number.$valid).toBe(data[2]);
            });
    });
}
