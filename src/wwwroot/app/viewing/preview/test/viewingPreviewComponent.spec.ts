/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingPreviewController = Component.ViewingPreviewController;

    describe('Given viewing preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            controller: ViewingPreviewController,
            state: ng.ui.IStateService;

        var pageObjectSelectors = {
            activitySection: '#activity-section',
            activityDetails: '#viewing-preview-activity-details',
            activityLinkDiv: '#activity-link',
            requirementSection: '#requirement-section',
            requirementDetails: '#viewing-preview-requirement-details',
            requirementLinkDiv: '#requirement-link',
            negotiatorDetails: '#viewing-preview-negotiator-details',
            invitationText: '#viewing-preview-invitation-text',
            postViewingComment: '#viewing-preview-post-viewing-comment',
            attendee: '#viewing-preview-attendees #viewing-preview-attendee-item-',
            date: '#viewing-preview-date'
        }

        describe('and proper viewing is provided', () => {
            var requirementMock = TestHelpers.RequirementGenerator.generate();
            var viewingMock = TestHelpers.ViewingGenerator.generate({
                requirement : requirementMock
            });
            

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService,
                $state: ng.ui.IStateService) =>{
                
                $http = $httpBackend;
                filter = $filter;
                state = $state;
                scope = $rootScope.$new();
                element = $compile('<viewing-preview></viewing-preview>')(scope);
                scope.$apply();

                controller = element.controller('viewingPreview');
            }));

            it('when viewing is set and page context is activity then requirement header should be displayed and activity header hidden', () => {
                // arrange
                controller.pageContext = 'activity';

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var requirementSection = element.find(pageObjectSelectors.requirementSection);
                var activitySection = element.find(pageObjectSelectors.activitySection);

                expect(requirementSection.length).toBe(1);
                expect(activitySection.length).toBe(0);
            });

            it('when viewing is set and page context is activity then requirement details should be displayed', () => {
                // arrange
                controller.pageContext = 'activity';

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var requirementDetailsElement = element.find(pageObjectSelectors.requirementDetails);
                var properRequirementTextToDisplay = requirementMock.getContactNames();
                expect(requirementDetailsElement.text()).toBe(properRequirementTextToDisplay);
            });

            it('when viewing is set and page context is activity then clicking details link should get requirement', () => {
                // arrange
                spyOn(state, 'go');
                controller.pageContext = 'activity';
                
                // act
                controller.setViewing(viewingMock);
                scope.$apply();
                
                // assert
                var requirementLink = element.find(pageObjectSelectors.requirementLinkDiv).children("a");
                requirementLink.click();
                expect(state.go).toHaveBeenCalledWith('app.requirement-view', { id: viewingMock.requirement.id });
            });            
            
            it('when viewing is set and page context is requirement then activity section should be displayed and requirement section hidden', () => {
                // arrange
                controller.pageContext = 'requirement';

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var activitySection = element.find(pageObjectSelectors.activitySection);
                var requirementSection = element.find(pageObjectSelectors.requirementSection);


                expect(requirementSection.length).toBe(0);
                expect(activitySection.length).toBe(1);
            });            

            it('when viewing is set and page context is requirement then activity details should be displayed', () => {
                // arrange
                controller.pageContext = 'requirement';

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var activityDetailsElement = element.find(pageObjectSelectors.activityDetails);
                var properActivityTextToDisplay = viewingMock.activity.property.address.getAddressText();
                expect(activityDetailsElement.text()).toBe(properActivityTextToDisplay);
            });

            it('when viewing is set and page context is requirement then clicking details link should get activity', () => {
                // arrange
                spyOn(state, 'go');
                controller.pageContext = 'requirement';

                // act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var activityLink = element.find(pageObjectSelectors.activityLinkDiv).children("a");
                activityLink.click();
                expect(state.go).toHaveBeenCalledWith('app.activity-view', { id: viewingMock.activity.id });
            });  
            
            it('when viewing is set then negotiator should be displayed', () => {
                // arrange + act
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert
                var negotiatorDetailsElement = element.find(pageObjectSelectors.negotiatorDetails);
                var properNegotiatorTextToDisplay = viewingMock.negotiator.getName();
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