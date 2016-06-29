/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferOfferDateControlConfig = Attributes.IOfferOfferDateControlConfig;

    describe('Given date edit control', () =>{
		var scope: ng.IScope,
		    element: ng.IAugmentedJQuery,
		    assertValidator: TestHelpers.AssertValidators;

        var format = (date: any) => moment(date).format('DD-MM-YYYY');
        var datesToTest: any = {
            today: new Date(2016,1,1),
            inThePast: new Date(2015,1,1),
            inTheFuture: new Date(2017,1,1)
        };

		var configMock: OfferOfferDateControlConfig = TestHelpers.ConfigGenerator.generateOfferOfferDateConfig();
        var schemaMock: Attributes.IDateEditControlSchema =
        {
            controlId : "mock-date",
            translationKey : "EDIT.MOCK_DATE",
            formName : "offerDateForm",
            fieldName : "offerDate"
        };

		var pageObjectSelectors = {
			control: "#date-edit-control",
            date: '#' + schemaMock.controlId,
            pickerButton: '#date-edit-control input-group button'
        };


	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
                scope['vm'] = { ngModel: datesToTest.today, config : configMock, schema : schemaMock };
                element = $compile('<date-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></date-edit-control>')(scope);
                scope.$apply();

				assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

	        it('then control is displayed', () =>{
	            var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
	            expect(controlElement.length).toBe(1);
            });

	        it('then proper date is set', () =>{
	            var dateElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.date);
	            expect(dateElement.val()).toBe(format(datesToTest.today));
	        });
	    });

        describe('has lower-than restriction', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                scope['vm'] = { ngModel: null, config: configMock, schema: schemaMock, lowerThan: datesToTest.today };
                element = $compile('<date-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema" lower-than="vm.lowerThan"></date-edit-control>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('when date value is lower than restriction limit then no validation message should be displayed', () =>{
                assertValidator.assertDateLowerThenValidator(format(datesToTest.inThePast), true, pageObjectSelectors.date);
            });

            it('when date value is greater than restriction limit then validation message should be displayed', () =>{
                assertValidator.assertDateLowerThenValidator(format(datesToTest.inTheFuture), false, pageObjectSelectors.date);
            });
        });

        describe('has greater-than restriction', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                scope['vm'] = { ngModel: null, config: configMock, schema: schemaMock, greaterThan: datesToTest.today };
                element = $compile('<date-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema" greater-than="vm.greaterThan"></date-edit-control>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('when date value is greater than restriction limit then no validation message should be displayed', () =>{
                assertValidator.assertDateGreaterThenValidator(format(datesToTest.inTheFuture), true, pageObjectSelectors.date);
            });

            it('when date value is lower than restriction limit then validation message should be displayed', () =>{
                assertValidator.assertDateGreaterThenValidator(format(datesToTest.inThePast), false, pageObjectSelectors.date);
            });
        });

        describe('is not configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
                scope['vm'] = { ngModel : datesToTest.today, schema : schemaMock };
                element = $compile('<price-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></price-edit-control>')(scope);
                scope.$apply();
            }));

            it('then control is not displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}