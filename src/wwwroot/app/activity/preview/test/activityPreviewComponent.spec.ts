/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityPreviewController = Activity.Preview.ActivityPreviewController;
    import Business = Common.Models.Business;

    describe('Given activity preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            controller: ActivityPreviewController;

        describe('and proper property id is provided', () => {
            var activityMock = TestHelpers.ActivityGenerator.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                $http = $httpBackend;
                filter = $filter;
                scope = $rootScope.$new();
                element = $compile('<activity-preview></activity-preview>')(scope);
                scope.$apply();

                controller = element.controller('activityPreview');
            }));

            it('when activity is set then activity status value should be displayed', () => {
                // arrange + act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var statusElement = element.find('#activity-status');
                expect(statusElement.text()).toBe('ENUMS.' + activityMock.activityStatusId);
            });

            it('when activity is set then activity creation date value should be displayed in proper format', () => {
                // arrange + act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var formattedDate = filter('date')(activityMock.createdDate, 'dd-MM-yyyy');
                var dateElement = element.find('#activity-created-date');
                expect(dateElement.text()).toBe(formattedDate);
            });

            it('when activity is set then activity vendors should be displayed', () => {
                // arrange
                var contact1Mock = new Business.Contact({ id : '11', firstName : 'John', surname : 'Test1', title : 'Mr' });//TestHelpers.ContactGenerator.generate();
                var contact2Mock = new Business.Contact({ id : '22', firstName : 'Amy', surname : 'Test2', title : 'Mrs' });//TestHelpers.ContactGenerator.generate();

                activityMock.contacts = [contact1Mock, contact2Mock];

                // act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var vendorsItemsElement1 = element.find('#activity-vendor-item-' + contact1Mock.id);
                var vendorsItemsElement2 = element.find('#activity-vendor-item-' + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText).toBe(contact2Mock.getName());
            });
        });
    });
}