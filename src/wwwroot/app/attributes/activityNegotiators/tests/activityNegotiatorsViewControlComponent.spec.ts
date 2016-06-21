/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import NegotiatorsController = Common.Component.ActivityNegotiatorsViewControlController;
	import IActivityViewConfig = Activity.IActivityViewConfig;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity negotiators view component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NegotiatorsController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            secondaryNegotiatorItems: '#card-list-negotiators [id^="negotiator-"]',
            leadNegotiatorItem: '#card-lead-negotiator [id^="negotiator-"]',
            noSecondaryNegotiators: '*[id="empty-secondary-negotiators"]',
            componentSorveyorMainHeader: 'h3[translate*="COMMERCIAL"]',
            componentSorveyorSubHeaders: 'h4[translate*="COMMERCIAL"]',
            componentNegotiatorMainHeader: 'h3[translate*="RESIDENTIAL"]',
            componentNegotiatorSubHeaders: 'h4[translate*="RESIDENTIAL"]'
        };

        var mockedComponentHtml = '<activity-negotiators-view-control config="config" property-division-id="{{propertyDivisionId}}" lead-negotiator="leadNegotiator" secondary-negotiators="secondaryNegotiators">'
            + '</negotiators-view>';
			
        var divisionCodes = [
            { id: "residentialId", code: "RESIDENTIAL" },
            { id: "commmercialId", code: "COMMERCIAL" }
        ];

		var configMock = { negotiators: {} } as IActivityViewConfig;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope.leadNegotiator = new Business.ActivityUser();
            scope.secondaryNegotiators = [];
	        scope['config'] = configMock;
            enumService.setEnum(Dto.EnumTypeCode.Division.toString(), divisionCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityNegotiatorsViewControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            var leadNegotiator = TestHelpers.ActivityUserGenerator.generate(Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator);
            var secondaryNegotiators = TestHelpers.ActivityUserGenerator.generateMany(3, Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator);

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

            describe('when secondary negotiators list is empty', () => {
                it('then no secondary negotiators message is visible', () => {
                    scope.secondaryNegotiators = null;
                    scope.$apply();

                    assertValidator.assertShowElement(false, pageObjectSelector.noSecondaryNegotiators);
                });
            });

            describe('when property division is set to \'Commercial\'', () => {
                it('then all component\'s headers should contain \'Surveyor\'', () => {
                    var division = _.find(divisionCodes, (division) =>  division.id === 'commmercialId');
                    controller.labelTranslationKey = division.code;
                    scope.$apply();

                    var mainHeader: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentSorveyorMainHeader);
                    var subHeaders: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentSorveyorSubHeaders);

                    expect(mainHeader.text()).toContain(division.code);;
                    _.each(subHeaders, (header) => expect(header.innerText).toContain(division.code));
                });
            });

            describe('when property division is set to \'Residential\'', () => {
                it('then all component\'s headers should contain \'Negotiator\'', () => {
                    var division = _.find(divisionCodes, (division) =>  division.id === 'residentialId');
                    controller.labelTranslationKey = division.code;
                    scope.$apply();

                    var mainHeader: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentNegotiatorMainHeader);
                    var subHeaders: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentNegotiatorSubHeaders);

                    expect(mainHeader.text()).toContain(division.code);
                    _.each(subHeaders, (header) => expect(header.innerText).toContain(division.code));
                });
            });
        });
    });
}