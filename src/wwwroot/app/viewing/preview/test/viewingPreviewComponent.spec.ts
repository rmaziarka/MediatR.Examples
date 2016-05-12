/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingPreviewController = Antares.Component.ViewingPreviewController;
    import Business = Common.Models.Business;

    describe('Given viewing preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            controller: ViewingPreviewController;

        var pageObjectSelectors = {
            activityDetails: '#viewing-preview-activity-details',
            negotiatorDetails: '#viewing-preview-negotiator-details',
            invitationText: '#viewing-preview-invitation-text',
            postViewingComment: '#viewing-preview-post-viewing-comment',
            attendee: '#viewing-preview-attendees #viewing-preview-attendee-item-',
            date: '#viewing-preview-date',
        }

        describe('and proper viewing is provided', () => {
            var viewingMock = TestHelpers.ViewingGenerator.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                $http = $httpBackend;
                filter = $filter;
                scope = $rootScope.$new();
                element = $compile('<viewing-preview></viewing-preview>')(scope);
                scope.$apply();

                controller = element.controller('viewingPreview');
            }));

            it('when viewing is set then activity should be displayed', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var activityDetailsElement = element.find(pageObjectSelectors.activityDetails);
                var properActiivtyTextToDisplay = viewingMock.activity.property.address.propertyName + (viewingMock.activity.property.address.propertyName ? ', ' : ' '
                    + viewingMock.activity.property.address.propertyNumber) + viewingMock.activity.property.address.propertyNumber + " " + viewingMock.activity.property.address.line2;
                expect(activityDetailsElement.text()).toBe(properActiivtyTextToDisplay);
            });
            
            it('when viewing is set then negotiator should be displayed', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var negotiatorDetailsElement = element.find(pageObjectSelectors.negotiatorDetails);
                var properNegotiatorTextToDisplay = viewingMock.negotiator.firstName + ' ' + viewingMock.negotiator.lastName;
                expect(negotiatorDetailsElement.text()).toBe(properNegotiatorTextToDisplay);
            });

            it('when viewing is set then invitation text should be displayed', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var invitationTextElement = element.find(pageObjectSelectors.invitationText);
                var proppeInvitationTextToDisplay = viewingMock.invitationText;
                expect(invitationTextElement.text()).toBe(proppeInvitationTextToDisplay);
            });

            it('when viewing is set then post viewing comment should be displayed', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var postViewingCommentElement = element.find(pageObjectSelectors.postViewingComment);
                var proppePostViewingCommentToDisplay = viewingMock.postViewingComment;
                expect(postViewingCommentElement.text()).toBe(proppePostViewingCommentToDisplay);
            });

            it('when viewing is set then viewing date and time period value should be displayed in proper format', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var formattedDate = filter('date')(viewingMock.startDate, 'dd-MM-yyyy');
                var formattedStartHour = filter('date')(viewingMock.startDate, 'HH:mm');
                var formattedEndHour = filter('date')(viewingMock.endDate, 'HH:mm');
                var properDateToDisplay = formattedDate + ", " + formattedStartHour + " - " + formattedEndHour;

                var dateElement = element.find(pageObjectSelectors.date);
                expect(dateElement.text()).toBe(properDateToDisplay);
            });

            it('when viewing is set then activity vendors should be displayed', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();

                viewingMock.attendees = [contact1Mock, contact2Mock];

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var attendeeItemsElement1 = element.find(pageObjectSelectors.attendee + contact1Mock.id);
                var attendeeItemsElement2 = element.find(pageObjectSelectors.attendee + contact2Mock.id);

                expect(attendeeItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(attendeeItemsElement2[0].innerText).toBe(contact2Mock.getName());
            });
        });
    });
}