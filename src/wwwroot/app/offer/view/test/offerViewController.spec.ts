/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferViewController = Component.OfferViewController;

    describe('Given offer view component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: OfferViewController,
            state: ng.ui.IStateService;

        var pageObjectSelectors = {
            vendorSection: '#section-vendor',
            applicantSection: '#section-applicant',
            cardItem: '.card-item',
            pageHeader: '#view-offer-header',
            pageHeaderTitle: '.offer-title',
            sectionBasicInformation: '#section-basic-information',
            applicant: '.applicant',
            panelBody: '.panel-body'
        }

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $state: ng.ui.IStateService) => {

            state = $state;
            scope = $rootScope.$new();
            element = $compile('<offer-view offer="offer"></offer-view>')(scope);
            scope.$apply();

            controller = element.controller('offerView');
        }));

        describe('when data are retrived form the server', () => {
            it('page header displays applicant names seppareted by comma', () => {
                var offerMock = TestHelpers.OfferGenerator.generate();

                //Add two contacts
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedHeaderNamesText = offerMock.requirement.getContactNames();
                var currentHeaderNames = element.find(pageObjectSelectors.pageHeader).find(pageObjectSelectors.pageHeaderTitle).text();
                
                expect(currentHeaderNames).toEqual(expectedHeaderNamesText);
            });

            it('vendor address is displayed within vendor card', () => {
                var offerMock = TestHelpers.OfferGenerator.generate();
                offerMock.activity.property.address.propertyNumber = "test property number";
                offerMock.activity.property.address.propertyName = "test property name";
                offerMock.activity.property.address.line2 = "test line2 address";

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedAddressText = offerMock.activity.property.address.getAddressText();
                var currentVendorAddressText = element.find(pageObjectSelectors.vendorSection).find(pageObjectSelectors.cardItem).text();

                expect(currentVendorAddressText).toEqual(expectedAddressText);
            });

            it('applicant names are displayed within applicant card seppareted by comma', () => {
                var offerMock = TestHelpers.OfferGenerator.generate();

                //Add two contacts
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedApplicantNamesText = offerMock.requirement.getContactNames();
                var currentApplicantNamesText = element.find(pageObjectSelectors.applicantSection).find(pageObjectSelectors.cardItem).text();

                expect(currentApplicantNamesText).toEqual(expectedApplicantNamesText);
            });
            
            it('negotiator name is displayed within negotiator card', () => {
                var offerMock = TestHelpers.OfferGenerator.generate();

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedNegotiatorFullNameText = offerMock.negotiator.getName();
                var currentNegotiatorFullName = element
                    .find(pageObjectSelectors.sectionBasicInformation)
                    .find(pageObjectSelectors.applicant)
                    .find(pageObjectSelectors.panelBody)
                    .text();

                expect(currentNegotiatorFullName).toEqual(expectedNegotiatorFullNameText);
            });
        });

        describe('when details button is clicked', () => {
            it('on context menu within vendor card then redirect to activity is performed', () => {
                var activityMock = TestHelpers.ActivityGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToActivity(activityMock);
                scope.$apply();

                var vendorDetailsLink = element.find(pageObjectSelectors.vendorSection).children("a");
                vendorDetailsLink.click();
                
                expect(state.go).toHaveBeenCalledWith('app.activity-view', { id: activityMock.id });
            });

            it('on context menu within applicant card then redirect to requiremen is performed', () => {
                var requirementMock = TestHelpers.RequirementGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToRequirement(requirementMock);
                scope.$apply();

                var applicantDetailsLink = element.find(pageObjectSelectors.applicantSection).children("a");
                applicantDetailsLink.click();

                expect(state.go).toHaveBeenCalledWith('app.requirement-view', { id: requirementMock.id });
            });
        });

    });
}