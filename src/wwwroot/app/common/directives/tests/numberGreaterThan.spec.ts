/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;

    interface INumberGreaterThanScope extends ng.IScope {
        number: number;
        form: any;
    }

    describe("Given input is valid with dateGreaterThan directive", () => {

        var element: ng.IAugmentedJQuery,
            scope: INumberGreaterThanScope,
            INumberGreaterThanScope: any;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $compile: ng.ICompileService) => {

            scope = <INumberGreaterThanScope>$rootScope.$new();

            scope['number'] = null;

            element = $compile(`
                <form name=\"form\">
                    <div>
                        <input type="number" id='number' number-greater-than="0" name='number' ng-model="number"/>
                    </div>
                </form>
                `)(scope);
            scope.$apply();
        }));

        type TestCaseForRequiredValidator = [number, boolean];

        runDescribe('when number')
            .data<TestCaseForRequiredValidator>([
                [0, false],
                [-1, false],
                [1, true]])
            .dataIt((data: TestCaseForRequiredValidator) =>
                `is set to "${data[0]}" then input should be ${data[1] ? 'valid' : 'invalid'}`)
            .run((data: TestCaseForRequiredValidator) => {
                // arrange / act / assert
                scope.form.number.$setViewValue(data[0]);
                scope.$apply();
                expect(scope.form.number.$valid).toBe(data[1]);
            });
    });
}
