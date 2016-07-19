/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    
    import NegotiatorsController = Antares.Attributes.ActivityNegotiatorsEditControlController;
    import IActivityEditConfig = Activity.IActivityEditConfig;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity negotiators edit component', () => {
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
            componentSorveyorMainHeader: 'h3[translate*="COMMERCIAL"]',
            componentSorveyorSubHeaders: 'h4[translate*="COMMERCIAL"]',
            componentNegotiatorMainHeader: 'h3[translate*="RESIDENTIAL"]',
            componentNegotiatorSubHeaders: 'h4[translate*="RESIDENTIAL"]',
            leadCallDate: '#lead-call-date'
        };

        var mockedComponentHtml = '<activity-negotiators-edit-control config="config" property-division-id="{{propertyDivisionId}}" lead-negotiator="leadNegotiator" secondary-negotiators="secondaryNegotiators">'
            + '</negotiators-edit>';
            
        var divisionCodes = [
            { id: "residentialId", code: "RESIDENTIAL" },
            { id: "commmercialId", code: "COMMERCIAL" }
        ];

        var configMock = { negotiators: {} } as IActivityEditConfig;

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
            controller = element.controller('activityNegotiatorsEditControl');

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

            it('with config it should display negotiators list', () => {
                var leadNegotiator = element.find(pageObjectSelector.leadNegotiator);
                expect(leadNegotiator.length).toEqual(1);
            });

            it('without config it should not display negotiators list', () => {
                scope['config'] = undefined;
                scope.$apply();
                var leadNegotiator = element.find(pageObjectSelector.leadNegotiator);
                expect(leadNegotiator.length).toEqual(0);
            });

            it('then it should display single lead negotiator entity', () => {
                var leadNegotiatorUserData: ng.IAugmentedJQuery = element.find(pageObjectSelector.leadNegotiatorItem);
                expect(leadNegotiatorUserData.text()).toEqual(leadNegotiator.user.firstName + ' ' + leadNegotiator.user.lastName);
            });

            xit('then list of secondary negotiators should be ordered by first and last name', () => {
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

                    assertValidator.assertElementHasHideClass(false, pageObjectSelector.noSecondaryNegotiators);
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

        describe('when negotiator call date is in the past', () => {
            beforeEach(() => {
                // init
                scope.leadNegotiator.callDate = moment().day(-7).toDate();
                scope.$apply();
            });

            it('and not changed then when save button is clicked save action should be called', () => {
                var form = controller.negotiatorsForm;
                form.$setSubmitted();
                scope.$apply();

                expect(form.$valid).toBe(true);
            });

            it('and changed also to past then when save button is clicked save action should not be called', () => {
                var newValueInThePast = moment().day(-10).format("DD-MM-YYYY");

                assertValidator.assertDateGreaterThenValidator(newValueInThePast, false, pageObjectSelector.leadCallDate);
            });
        });
    });
}