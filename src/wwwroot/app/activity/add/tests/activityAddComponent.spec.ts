/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddController = Activity.ActivityAddController;

    describe('Given activity is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: ActivityAddController;

        var pageObjectSelectors = {
            activityStatusSelector: 'select#status'
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