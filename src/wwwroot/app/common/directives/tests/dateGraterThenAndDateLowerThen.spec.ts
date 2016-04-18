/// <reference path="../../../typings/_all.d.ts" />
namespace Antares {

    describe("Given input's are valid with dateGraterThen and dateLowerThen direcitves", () => {

        var element: ng.IAugmentedJQuery,
            scope: ng.IScope,
            originalUtilsCreateDateAsUtc: any,
            // required because phantomjs can't create date object from iso string value.
            mockDateValues: any = {
                "12-12-2000": () => originalUtilsCreateDateAsUtc('Dec 12 2000'),
                "12-12-2001": () => originalUtilsCreateDateAsUtc('Dec 12 2001'),
                "20-12-2001": () => originalUtilsCreateDateAsUtc('Dec 20 2001')
            }, 
            sellDateAdapter:Antares.TestHelpers.InputValidationAdapter, 
            purchaseDateAdapter:Antares.TestHelpers.InputValidationAdapter;

        beforeAll(() => {
            originalUtilsCreateDateAsUtc = Antares.Core.DateTimeUtils.createDateAsUtc;
        });

        afterAll(() => {
            Antares.Core.DateTimeUtils.createDateAsUtc = originalUtilsCreateDateAsUtc;
        });

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {
            scope = $rootScope.$new();
            scope['sellDate'] = '';
            scope['purchaseDate'] = '';
            element = $compile(`
                <div>
                    <input type="text" id='purchaseDate' date-lower-than="{{sellDate}}"  ng-model="purchaseDate"/>
                    <input type="text" id='sellDate' date-greater-than="{{purchaseDate}}"  ng-model="sellDate"/>
                </div>
                `)(scope);
            scope.$apply();

            var sellDateEl: ng.IAugmentedJQuery = element.find('#sellDate');
            var purchaseDateEl: ng.IAugmentedJQuery = element.find('#purchaseDate');

            sellDateAdapter = new Antares.TestHelpers.InputValidationAdapter(sellDateEl, null, scope);
            purchaseDateAdapter = new Antares.TestHelpers.InputValidationAdapter(purchaseDateEl, null, scope);

            spyOn(Antares.Core.DateTimeUtils, 'createDateAsUtc').and.callFake((date: string) => {
                return mockDateValues[date]();
            });
        }));

        it('should be invalid when lower date is grater then "grater date"', () => {
            sellDateAdapter.writeValue('12-12-2001');
            purchaseDateAdapter.writeValue('12-12-2000');

            expect(sellDateAdapter.isInputValid()).toBeTruthy();
        });

        it('should be valid when lower date and "grater date" are same', () => {

            sellDateAdapter.writeValue('12-12-2001');
            purchaseDateAdapter.writeValue('12-12-2001');

            expect(sellDateAdapter.isInputValid()).toBeTruthy();
            expect(purchaseDateAdapter.isInputValid()).toBeTruthy();
        });

        it('should be invalid when "lower date" is grater then "grater date" after change from same date', () => {

            sellDateAdapter.writeValue('12-12-2001');
            purchaseDateAdapter.writeValue('12-12-2001');
            purchaseDateAdapter.writeValue('20-12-2001');

            expect(purchaseDateAdapter.isInputValid()).toBeFalsy();
        });
    });
}