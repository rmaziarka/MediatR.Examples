/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import NegotiatorsController = Antares.Attributes.ContactNegotiatorsEditControlController;
	
    interface IScope extends ng.IScope {
        contactId: number;
        leadNegotiator: Business.ContactUser;
        secondaryNegotiators: Business.ContactUser[];
      }

    describe('Given contact negotiators edit component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NegotiatorsController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            leadNegotiator: '#card-lead-negotiator',
            secondaryNegotiatorItems: '#card-list-negotiators [id^="negotiator-"]',
            leadNegotiatorItem: '#card-lead-negotiator [id^="negotiator-"]',
            noSecondaryNegotiators: '*[id="empty-secondary-negotiators"]',
            editLeadNegotiator:'#lead-edit-btn-wrapper #lead-edit-btn i.fa'
         };

        var mockedComponentHtml = '<contact-negotiators-edit-control contact-id="contactId" lead-negotiator="leadNegotiator" secondary-negotiators="secondaryNegotiators">'
            + '</contact-negotiators-edit-control>';
			
       beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope.leadNegotiator = new Business.ContactUser();
            scope.secondaryNegotiators = [];
	     
            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('contactNegotiatorsEditControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            var leadNegotiator = TestHelpers.ContactUserGenerator.generate(Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator);
            var secondaryNegotiators = TestHelpers.ContactUserGenerator.generateMany(3, Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator);

            beforeEach(() => {
                scope.leadNegotiator = leadNegotiator;
                scope.secondaryNegotiators = secondaryNegotiators;
                scope.$apply();
            });

            it('then it should display single lead negotiator entity', () => {
                var leadNegotiatorUserData: ng.IAugmentedJQuery = element.find(pageObjectSelector.leadNegotiatorItem);
                expect(leadNegotiatorUserData.text()).toEqual(leadNegotiator.user.firstName + ' ' + leadNegotiator.user.lastName);
            });

            it('then list of secondary negotiators should be ordered by first and last name', () => {
                secondaryNegotiators[0].user.firstName = 'b';
                secondaryNegotiators[0].user.lastName = 'a';

                secondaryNegotiators[1].user.firstName = 'A';
                secondaryNegotiators[1].user.lastName = 'b';

                secondaryNegotiators[2].user.firstName = 'a';
                secondaryNegotiators[2].user.lastName = 'a';

                scope.$apply();

                var elementNegotiatorList: ng.IAugmentedJQuery = element.find(pageObjectSelector.secondaryNegotiatorItems);

                expect(elementNegotiatorList[0].innerText).toBe(secondaryNegotiators[2].user.getName());
                expect(elementNegotiatorList[1].innerText).toBe(secondaryNegotiators[1].user.getName());
                expect(elementNegotiatorList[2].innerText).toBe(secondaryNegotiators[0].user.getName());
             });

            describe('when lead negotiators is not set', () =>{
                beforeEach(() =>{
                    scope.leadNegotiator = null;
                    scope.$apply();
                });

                it('then lead negotiator should not be displayed', () => {
                  
                    var leadNegotiatorUserData = element.find(pageObjectSelector.leadNegotiatorItem);
                    expect(leadNegotiatorUserData.text()).toEqual(" ");                  
                });

                it('and edit negotiator button is displayed', () => {
                   var leadNegotiatorEditElement: ng.IAugmentedJQuery = element.find(pageObjectSelector.editLeadNegotiator);
                    expect(leadNegotiatorEditElement.length).toBeGreaterThan(0);
                });
            });

            describe('when secondary negotiators list is empty', () => {
                it('then no secondary negotiators message is visible', () => {
                    scope.secondaryNegotiators = null;
                    scope.$apply();

                    assertValidator.assertShowElement(false, pageObjectSelector.noSecondaryNegotiators);
                });
            });

        });
    });
}