/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    interface IGreaterThanScope extends ng.IScope {
        model: any;
        form: any;
        sellDate: any;
        purchaseDate: any;
    }
    describe("Given input's are valid with dateGreaterThan and dateLowerThan direcitves", () => {

        var element: ng.IAugmentedJQuery,
            scope: IGreaterThanScope,
            originalUtilsCreateDateAsUtc: any,
            // required because phantomjs can't create date object from iso string value.
            mockDateValues: any = {
                "12-12-2000": () => originalUtilsCreateDateAsUtc('Dec 12 2000'),
                "12-12-2001": () => originalUtilsCreateDateAsUtc('Dec 12 2001'),
                "20-12-2001": () => originalUtilsCreateDateAsUtc('Dec 20 2001')
            };

        beforeAll(() => {
            originalUtilsCreateDateAsUtc = Antares.Core.DateTimeUtils.createDateAsUtc;
        });

        afterAll(() => {
            Antares.Core.DateTimeUtils.createDateAsUtc = originalUtilsCreateDateAsUtc;
        });

        describe('when "allowEquality" attribute is not specified', () => {
            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
                scope = <IGreaterThanScope>$rootScope.$new();
                scope['sellDate'] = '';
                scope['purchaseDate'] = '';
                element = $compile(`
                <form name=\"form\">
                    <div>
                        <input type="text" id='purchaseDate' date-lower-than="{{sellDate}}" name='purchaseDate' ng-model="purchaseDate"/>
                        <input type="text" id='sellDate' date-greater-than="{{purchaseDate}}" name='sellDate' ng-model="sellDate"/>
                    </div>
                </form>
                `)(scope);
                scope.$apply();

                var sellDateEl: ng.IAugmentedJQuery = element.find('#sellDate');
                var purchaseDateEl: ng.IAugmentedJQuery = element.find('#purchaseDate');

                spyOn(Antares.Core.DateTimeUtils, 'createDateAsUtc').and.callFake((date: string) => {
                    return mockDateValues[date]();
                });
            }));

            it('then form must be valid', () => {
                scope.form.purchaseDate.$setViewValue(new Date(2000, 10, 2));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 2));
                scope.$apply();
                expect(scope.form.purchaseDate.$valid).toBe(true);
                expect(scope.form.sellDate.$valid).toBe(true);
            });
        });

        describe('when both dates can be greater or equal', () => {
            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
                scope = <IGreaterThanScope>$rootScope.$new();
                scope['sellDate'] = '';
                scope['purchaseDate'] = '';
                element = $compile(`
                <form name=\"form\">
                    <div>
                        <input type="text" id='purchaseDate' date-lower-than="{{sellDate}}" name='purchaseDate' ng-model="purchaseDate"/>
                        <input type="text" id='sellDate' date-greater-than="{{purchaseDate}}" name='sellDate' ng-model="sellDate" allow-equality="true"/>
                    </div>
                </form>
                `)(scope);
                scope.$apply();

                var sellDateEl: ng.IAugmentedJQuery = element.find('#sellDate');
                var purchaseDateEl: ng.IAugmentedJQuery = element.find('#purchaseDate');

                spyOn(Antares.Core.DateTimeUtils, 'createDateAsUtc').and.callFake((date: string) => {
                    return mockDateValues[date]();
                });
            }));

            it('should be invalid when lower date is greater than "greater date"', () => {
                scope.form.purchaseDate.$setViewValue(new Date(2000, 10, 2));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 1));
                scope.$apply();
                expect(scope.form.purchaseDate.$valid).toBeFalsy();
                expect(scope.form.sellDate.$valid).toBeFalsy();
            });

            it('should be valid when lower date and "greater date" are same', () => {
                scope.form.purchaseDate.$setViewValue(new Date(2000, 10, 1));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 1));
                scope.$apply();
                expect(scope.form.purchaseDate.$valid).toBeTruthy();
                expect(scope.form.sellDate.$valid).toBeTruthy();
            });

            it('should be invalid when "lower date" is greater than "greater date" after change from same date', () => {
                scope.form.purchaseDate.$setViewValue(new Date(2000, 10, 2));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 2));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 1));
                scope.$apply();
                expect(scope.form.purchaseDate.$valid).toBeFalsy();
                expect(scope.form.sellDate.$valid).toBeFalsy();
            });
        });

        describe('when both dates can not be equal', () => {
            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
                scope = <IGreaterThanScope>$rootScope.$new();
                scope['sellDate'] = '';
                scope['purchaseDate'] = '';
                element = $compile(`
                <form name=\"form\">
                    <div>
                        <input type="text" id='purchaseDate' date-lower-than="{{sellDate}}" name='purchaseDate' ng-model="purchaseDate"/>
                        <input type="text" id='sellDate' date-greater-than="{{purchaseDate}}" name='sellDate' ng-model="sellDate" allow-equality="false"/>
                    </div>
                </form>
                `)(scope);
                scope.$apply();

                var sellDateEl: ng.IAugmentedJQuery = element.find('#sellDate');
                var purchaseDateEl: ng.IAugmentedJQuery = element.find('#purchaseDate');

                spyOn(Antares.Core.DateTimeUtils, 'createDateAsUtc').and.callFake((date: string) => {
                    return mockDateValues[date]();
                });
            }));
            it('when equality is not allowed then field "sellDate" must be invalid', () => {
                scope.form.purchaseDate.$setViewValue(new Date(2000, 10, 2));
                scope.form.sellDate.$setViewValue(new Date(2000, 10, 2));
                scope.$apply();
                expect(scope.form.sellDate.$valid).toBeFalsy();
            });
        });
    });

    describe('should be invalid when "lower date" is greater than "greater date" after change from same date', () => { });
}