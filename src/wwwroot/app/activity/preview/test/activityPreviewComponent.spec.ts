/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityPreviewController = Antares.Activity.Preview.ActivityPreviewController;
    import Activity = Common.Models.Dto.Activity;
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
                activityStatusId: 'testSatusId',
                //TODO: test data generator should be implemented to simplifi mockig data
                contacts: [
                    <Contact>{ id: '11', firstName: 'John', surname: 'Test1', title: 'Mr' },
                    <Contact>{ id: '22', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }],
                createdDate: new Date()
            };

            beforeEach(angular.mock.module('app'));
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
                controller.setActivity(activityMock);
                scope.$apply();

                var statusElement = element.find('.activity-status');
                expect(statusElement.text()).toBe('ENUMS.' + activityMock.activityStatusId);
            });

            it('when activity is set then activity creation date value should be displayed in proper format', () => {
                controller.setActivity(activityMock);
                scope.$apply();

                var formattedDate = filter('date')(activityMock.createdDate, 'dd-MM-yyyy');
                var dateElement = element.find('.activity-created-date');
                expect(dateElement.text()).toBe(formattedDate);
            });

            it('when activity is set then activity vendors should be displayed', () => {
                controller.setActivity(activityMock);
                scope.$apply();

                var vendorsItemsElements = element.find('.activity-vendor-item');

                expect(vendorsItemsElements[0].innerText).toBe('John Test1');
                expect(vendorsItemsElements[1].innerText).toBe('Amy Test2');
            });
        });

    });
}