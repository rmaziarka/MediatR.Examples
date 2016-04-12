/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IKfMaxDateScope extends ng.IScope {
        date: any
    }
    describe('Given input validate max date with directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IKfMaxDateScope,
            assertValidator: TestHelpers.AssertValidators;

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {
            scope = <IKfMaxDateScope>$rootScope.$new();
            scope.date = '';
            element = $compile('<div><input type="text" kf-max-date="Jun 01 2000" ng-model="date"/></div>')(scope);
            scope.$apply();
        }));

        describe('when max date is set ', () => {

            it('should mark field invalid ', () => {
                var input = element.find('input');
                input.val('Oct 01 2000').trigger('input').trigger('change').trigger('blur');
                scope.$apply();
                expect(input.hasClass('ng-invalid')).toBe(true);
            });

            it('should mark field as valid', () => {
                var input = element.find('input');
                input.val('Jun 01 2000').trigger('input').trigger('change').trigger('blur');
                scope.$apply();
                expect(input.hasClass('ng-valid')).toBe(true);
            });

            it('should mark field as valid for empty date', () => {
                var input = element.find('input');
                input.val('').trigger('input').trigger('change').trigger('blur');
                scope.$apply();
                expect(input.hasClass('ng-valid')).toBe(true);
            });

        });
        describe(' when max date is not set', () => {
            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
                scope = <IKfMaxDateScope>$rootScope.$new();
                scope.date = '';
                element = $compile('<div><input type="text" kf-max-date="" ng-model="date"/></div>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('should mark field as valid for any date', () => {
                var input = element.find('input');
                input.val('Oct 10 1010').trigger('input').trigger('change').trigger('blur');
                scope.$apply();

                expect(input.hasClass('ng-valid')).toBe(true);

            });

            it('should mark field as valid for empty date', () => {
                var input = element.find('input');
                input.val('').trigger('input').trigger('change').trigger('blur');
                scope.$apply();

                expect(input.hasClass('ng-valid')).toBe(true);

            });
        });
    });

    describe("Given input's are valid with dateGraterThen and dateLowerThen direcitves", () => {

        var element: ng.IAugmentedJQuery,
            scope: any,
            assertValidator: TestHelpers.AssertValidators,
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

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {
            scope = $rootScope.$new();
            scope.sellDate = '';
            scope.purchaseDate = '';
            element = $compile(`
                <div>
                    <input type="text" id='purchaseDate' date-lower-than="{{sellDate}}"  ng-model="purchaseDate"/>
                    <input type="text" id='sellDate' date-greater-than="{{purchaseDate}}"  ng-model="sellDate"/>
                </div>
                `)(scope);
            scope.$apply();

            spyOn(Antares.Core.DateTimeUtils, 'createDateAsUtc').and.callFake((date: string) => {
                return mockDateValues[date]();
            });
        }));


        afterAll(() => {
            Antares.Core.DateTimeUtils.createDateAsUtc = originalUtilsCreateDateAsUtc;
        });

        it('should be invalid when lower date is grater then "grater date"', () => {
            var sellDateEl = element.find('#sellDate').first();
            var purchaseDateEl = element.find('#purchaseDate').first();

            purchaseDateEl.val('12-12-2000').trigger('input').trigger('change').trigger('blur');
            sellDateEl.val('12-12-2001').trigger('input').trigger('change').trigger('blur');
            scope.$apply();

            expect(sellDateEl.hasClass('ng-valid')).toBeTruthy();
        });

        it('should be valid when lower date and "grater date" are same', () => {
            var sellDateEl = element.find('#sellDate').first();
            var purchaseDateEl = element.find('#purchaseDate').first();

            purchaseDateEl.val('12-12-2001').trigger('input').trigger('change').trigger('blur');
            sellDateEl.val('12-12-2001').trigger('input').trigger('change').trigger('blur');
            scope.$apply();

            expect(sellDateEl.hasClass('ng-valid')).toBeTruthy();
            expect(purchaseDateEl.hasClass('ng-valid')).toBeTruthy();
        });

        it('should be invalid when lower date is grater then "grater date" after change from same date', () => {

            var sellDateEl = element.find('#sellDate').first();
            var purchaseDateEl = element.find('#purchaseDate').first();

            purchaseDateEl.val('12-12-2001').trigger('input').trigger('change').trigger('blur');
            sellDateEl.val('12-12-2001').trigger('input').trigger('change').trigger('blur');
            scope.$apply();
            purchaseDateEl.val('20-12-2001').trigger('input').trigger('change').trigger('blur');
            scope.$apply();

            expect(purchaseDateEl.hasClass('ng-invalid')).toBeTruthy();
        });
    });
}