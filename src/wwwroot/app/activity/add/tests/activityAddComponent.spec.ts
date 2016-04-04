/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddController = Activity.ActivityAddController;
    import Dto = Antares.Common.Models.Dto;

    describe('Given activity is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: ActivityAddController;

        var pageObjectSelectors = {
            activityStatusSelector: 'select#status',
            vendors: '#vendors'
        };

        var activityStatuses: any = {
            items:
            [
                { id: "1", value: "Pre-appraisal", code: "PreAppraisal" },
                { id: "2", value: "Market appraisal", code: "MarketAppraisal" },
                { id: "3", value: "Not selling", code: "NotSelling" }
            ]
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            compile = $compile;

            $http.expectGET(/\/api\/enums\/ActivityStatus\/items/).respond(() => {
                return [200, activityStatuses];
            });

            scope = $rootScope.$new();
            element = compile('<activity-add></activity-add>')(scope);
            scope.$apply();
            $http.flush();
            
            controller = element.controller('activityAdd');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }
        ));

        describe('when component is being loaded', () => {
            it('then default Pre-appraisal status is selected', () => {
                var expectedPreAppraisalStatus = _.find(activityStatuses.items, { 'code': 'PreAppraisal' });
                expect(controller.selectedActivityStatus).toEqual(expectedPreAppraisalStatus);
            });
        });

        describe('when vendors are set in component', () => {
            var setVendors = (vendorsCount: number): Dto.Contact[] => {
                var vendors = _.map(Antares.TestHelpers.ContactGenerator.GenerateMany(vendorsCount), (dtoContact: Dto.IContact) => {
                    return new Dto.Contact(dtoContact);
                });

                controller.setVendors(vendors);
                scope.$apply();

                return vendors;
            }

            it('then all should be displayed', () => {
                var vendorsCount: number = 3;
                setVendors(vendorsCount);

                var elements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.vendors);
                expect(elements.length).toBe(vendorsCount);
            });

            it('then his name should be displayed correctly', () => {
                var vendorsCount: number = 1;
                var vendor: Dto.Contact = setVendors(vendorsCount)[0];

                var elements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.vendors);
                expect(elements.length).toBe(1);

                var vendorElement: HTMLElement = elements[0];
                expect(vendorElement.innerText).toBe(vendor.getName());
            });
        });

        describe('when activity status value is ', () => {
            it('missing then required message should be displayed', () => {
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.activityStatusSelector);
            });

            it('not missing then required message should not be displayed', () => {
                assertValidator.assertRequiredValidator('2', true, pageObjectSelectors.activityStatusSelector);
            });
        });
    });
}