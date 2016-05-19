/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddController = Activity.ActivityAddController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;

    describe('Given activity is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: ActivityAddController;

        var pageObjectSelectors = {
            activityStatusSelector: 'enum-select#status select',
            activityTypeSelector: 'select#type',
            vendors: '#activity-add-vendors [id^=list-item-]'
        };

        var activityTypes: any =
            [
                { id: "1", order: 1 },
                { id: "2", order: 2 },
                { id: "3", order: 3 }
            ];

        var activityStatuses = [
            { id: "111", code: "PreAppraisal" },
            { id: "testStatus222", code: "MarketAppraisal" },
            { id: "333", code: "NotSelling" }
        ];

        var createVendors = (count: number) => {
            return _.map(TestHelpers.ContactGenerator.generateMany(count), (dtoContact: Dto.IContact) => {
                return new Business.Contact(dtoContact);
            });
        }

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            compile = $compile;

            $http.expectGET(/\/api\/activities\/types/).respond(() => {
                return [200, activityTypes];
            });

            // enums
            enumService.setEnum(Dto.EnumTypeCode.ActivityStatus.toString(), activityStatuses);

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
                var expectedPreAppraisalStatus = _.find(activityStatuses, { 'code': 'PreAppraisal' });
                expect(controller.selectedActivityStatusId).toEqual(expectedPreAppraisalStatus.id);
            });
        });

        describe('when vendors are set in component', () => {
            var setVendors = (vendorsCount: number): Business.Contact[] => {
                var vendors = createVendors(vendorsCount);

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
                var vendor: Business.Contact = setVendors(vendorsCount)[0];

                var elements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.vendors);
                expect(elements.length).toBe(1);

                var vendorElement: HTMLElement = elements[0];
                expect(vendorElement.innerText.trim()).toBe(vendor.getName());
            });
        });

        type TestCaseForRequiredValidator = [string, boolean];
        // RequiredValidator for activity status
        runDescribe('when activity status ')
            .data<TestCaseForRequiredValidator>([
                [activityStatuses[2].id, true],
                ['statusIdThatIsNotOnTheList', false],
                ['', false],
                [null, false]])
            .dataIt((data: TestCaseForRequiredValidator) =>
                `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForRequiredValidator) => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.activityStatusSelector);
            });

        // RequiredValidator for activity type
        runDescribe('when activity status ')
            .data<TestCaseForRequiredValidator>([
                [activityTypes[1].id, true],
                ['typeIdThatIsNotOnTheList', false],
                ['', false],
                [null, false]])
            .dataIt((data: TestCaseForRequiredValidator) =>
                `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForRequiredValidator) => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.activityTypeSelector);
            });
    });
}