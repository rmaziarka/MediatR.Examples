/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityPreviewController = Antares.Activity.Preview.ActivityPreviewController;
    import Activity = Common.Models.Business.Activity;
    import Contact = Common.Models.Dto.Contact;

    describe('Given activity preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService;

        var controller: ActivityPreviewController;


        describe('and proper property id is provided', () => {
            var activityMock: Activity = {
                id: '999',
                propertyId: '111',
                property: null,
                activityStatusId: 'testSatusId',
                //TODO: test data generator should be implemented to simplifi mockig data
                contacts: [
                    <Contact>{ id: '11', firstName: 'John', surname: 'Test1', title: 'Mr' },
                    <Contact>{ id: '22', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }],
                createdDate: new Date()
            };
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
                // arrange + act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var vendorsItemsElement1 = element.find('#activity-vendor-item-11');
                var vendorsItemsElement2 = element.find('#activity-vendor-item-22');

                expect(vendorsItemsElement1[0].innerText).toBe('John Test1');
                expect(vendorsItemsElement2[0].innerText).toBe('Amy Test2');
            });
        });

    });
}