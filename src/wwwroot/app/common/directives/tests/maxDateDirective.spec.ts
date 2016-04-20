/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IKfMaxDateScope extends ng.IScope {
        date: any
    }

    describe('Given input validate max date with directive', () => {
        var element: ng.IAugmentedJQuery,
            scope: IKfMaxDateScope,
            assertValidator: TestHelpers.AssertValidators,
            originalUtilsConvertDateAsUtc: any,
            // required because phantomjs can't create date object from iso string value.
            mockDateValues: any = {
                "10-10-1010": () => originalUtilsConvertDateAsUtc('Oct 10 1010'),
                "01-01-2000": () => originalUtilsConvertDateAsUtc('Jan 01 2000'),
                "10-01-2000": () => originalUtilsConvertDateAsUtc('Jan 10 2000'),
                "12-12-2001": () => originalUtilsConvertDateAsUtc('Dec 12 2001'),
                "20-12-2001": () => originalUtilsConvertDateAsUtc('Dec 20 2001')
            };

        beforeAll(() => {
            originalUtilsConvertDateAsUtc = Antares.Core.DateTimeUtils.convertDateToUtc;
        });

        afterAll(() => {
            Antares.Core.DateTimeUtils.createDateAsUtc = originalUtilsConvertDateAsUtc;
        });

        beforeEach(inject(($rootScope: ng.IRootScopeService, $compile: ng.ICompileService) => {

          

            scope = <IKfMaxDateScope>$rootScope.$new();
            scope.date = '';
            element = $compile('<div><input type="text" kf-max-date="01-01-2000" ng-model="date"/></div>')(scope);
            scope.$apply();

        }));

        describe('when max date is set ', () => {

            it('should mark field invalid ', () => {
                  spyOn(Antares.Core.DateTimeUtils, 'convertDateToUtc').and.callFake((date: string) => {
                return mockDateValues[date]();
            });
                var input = element.find('input');
                input.val('10-01-2000').trigger('input').trigger('change').trigger('blur');
                scope.$apply();
                expect(input.hasClass('ng-invalid')).toBe(true);
            });

            it('should mark field as valid', () => {
                var input = element.find('input');
                input.val('01-01-2000').trigger('input').trigger('change').trigger('blur');
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
                input.val('10-10-1010').trigger('input').trigger('change').trigger('blur');
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
}