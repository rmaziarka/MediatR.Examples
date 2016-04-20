﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityPreviewController = Activity.Preview.ActivityPreviewController;
    import Business = Common.Models.Business;

    describe('Given activity preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            controller: ActivityPreviewController;

        var pageObjectSelectors = {
            createdDate: '#activity-preview-created-date',
            status: '#activity-preview-status',
            vendor: '#activity-preview-vendors #activity-preview-vendor-item-'
        }

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
                var statusElement = element.find(pageObjectSelectors.status);
                expect(statusElement.text()).toBe('DYNAMICTRANSLATIONS.' + activityMock.activityStatusId);
            });

            it('when activity is set then activity creation date value should be displayed in proper format', () => {
                // arrange + act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var formattedDate = filter('date')(activityMock.createdDate, 'dd-MM-yyyy');
                var dateElement = element.find(pageObjectSelectors.createdDate);
                expect(dateElement.text()).toBe(formattedDate);
            });

            it('when activity is set then activity vendors should be displayed', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();

                activityMock.contacts = [contact1Mock, contact2Mock];

                // act
                controller.setActivity(activityMock);
                scope.$apply();

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.vendor + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.vendor + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText).toBe(contact2Mock.getName());
            });
        });
    });
}