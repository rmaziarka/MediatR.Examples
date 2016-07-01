/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferOfferDateControlConfig = Attributes.IOfferOfferDateControlConfig;

    describe('Given date view control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var format = (date: any) => moment(date).format('DD-MM-YYYY');
        var dateToTest = new Date(2016, 1, 1);

        var configMock: OfferOfferDateControlConfig = TestHelpers.ConfigGenerator.generateOfferOfferDateConfig();
        var schemaMock: Attributes.IDateControlSchema =
            {
                controlId: "mock-date",
                translationKey: "VIEW.MOCK_DATE"
            };

        var pageObjectSelectors = {
            control: '#' + schemaMock.controlId + '-section',
            date: '#' + schemaMock.controlId + ' time'
        };

        describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                scope['vm'] = { ngModel: dateToTest, config: configMock, schema: schemaMock };
                element = $compile('<date-view-control date="vm.ngModel" config="vm.config" schema="vm.schema"></date-view-control>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('then control is displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(1);
            });

            it('then proper date is set', () => {
                var dateElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.date);
                expect(dateElement.text()).toBe(format(dateToTest));
            });
        });

        describe('is not configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                scope['vm'] = { ngModel: dateToTest, schema: schemaMock };
                element = $compile('<price-view-control date="vm.ngModel" config="vm.config" schema="vm.schema"></price-view-control>')(scope);
                scope.$apply();
            }));

            it('then control is not displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}