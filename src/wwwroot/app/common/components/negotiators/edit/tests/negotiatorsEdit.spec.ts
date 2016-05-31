/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import NegotiatorsController = Antares.Common.Component.NegotiatorsController;
    import SearchController = Common.Component.SearchController;
    import runDescribe = TestHelpers.runDescribe;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiator: Business.ActivityUser[];
        activityId: string;
        propertyDivisionId: string;
    }

    describe('Given negotiators edit component', () => {
        var pageObjectSelector = {
            secondaryNegotiatorItems: '#card-list-negotiators [id^="negotiator-"]',
            leadNegotiatorItem: '#card-lead-negotiator [id^="negotiator-"]',
            noSecondaryNegotiators: '*[id="empty-secondary-negotiators"]',
            componentSorveyorMainHeader: 'h3[translate*="COMMERCIAL"]',
            componentSorveyorSubHeaders: 'h4[translate*="COMMERCIAL"]',
            componentNegotiatorMainHeader: 'h3[translate*="RESIDENTIAL"]',
            componentNegotiatorSubHeaders: 'h4[translate*="RESIDENTIAL"]',
            leadSearch: 'search#lead-search',
            leadCallDate: 'input#lead-call-date',
            secondarySearch: 'search#secondary-search',
            secondaryCallDatePrefix: 'input#call-date-',
            editLeadNegotiatorBtn: '#lead-edit-btn',
            editLeadNegotiatorBtnWrapper: '#lead-edit-btn-wrapper',
            addSecondaryBtn: '#card-list-negotiators button[data-type="addItem"]',
            editSecondaryNegotiatorsBtn: 'card-list#card-list-negotiators button#addItemBtn:first'
        };

        var format = (date: any) => date.format('DD-MM-YYYY');
        var datesToTest: any = {
            today: format(moment()),
            inThePast: format(moment().day(-7)),
            longAgo: format(moment().year(1700)),
            inTheFuture: format(moment().day(7))
        };

        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NegotiatorsController,
            assertValidator: TestHelpers.AssertValidators,
            searchController: SearchController;

        var mockedComponentHtml = '<negotiators-edit property-division-id="{{propertyDivisionId}}" activity-id="activityId" lead-negotiator="leadNegotiator" secondary-negotiators="secondaryNegotiator">'
            + '</negotiators-edit>';

        var divisionCodes = [
            { id: "residentialId", code: "RESIDENTIAL" },
            { id: "commmercialId", code: "COMMERCIAL" }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope.leadNegotiator = new Business.ActivityUser();
            scope.secondaryNegotiator = [];

            enumService.setEnum(Dto.EnumTypeCode.Division.toString(), divisionCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('negotiatorsEdit');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when activity is edited', () => {
            var leadNegotiator = TestHelpers.ActivityUserGenerator.generate(Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator);
            var secondaryNegotiators = TestHelpers.ActivityUserGenerator.generateMany(3, Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator);

            beforeEach(() => {
                scope.leadNegotiator = leadNegotiator;
                scope.secondaryNegotiator = secondaryNegotiators;
                scope.$apply();
            });

            it('then it should display single lead negotiator entity', () => {
                var leadNegotiatorUserData: ng.IAugmentedJQuery = element.find(pageObjectSelector.leadNegotiatorItem);
                expect(leadNegotiatorUserData.text()).toEqual(leadNegotiator.user.firstName + ' ' + leadNegotiator.user.lastName);
            });

            it('then list of secondary negotiators should be ordered by first and last name', () => {
                var elementNegotiatorList: ng.IAugmentedJQuery = element.find(pageObjectSelector.secondaryNegotiatorItems);
                var expectedOrder: Business.ActivityUser[] = _.sortByOrder(secondaryNegotiators, ['user.firstName', 'user.lastName']);

                var isOrdered: boolean = _.every(elementNegotiatorList, (itm, ind, coll): boolean => {
                    var sourceEl: string = expectedOrder[ind].user.firstName + ' ' + expectedOrder[ind].user.lastName;

                    return itm.innerText === sourceEl;
                });

                expect(isOrdered).toBeTruthy();
            });

            describe('when secondary negotiators list is empty', () => {
                it('then no secondary negotiators id visible', () => {
                    scope.secondaryNegotiator = null;
                    scope.$apply();

                    assertValidator.assertShowElement(false, pageObjectSelector.noSecondaryNegotiators);
                });
            });

            describe('when property division is set to \'Commercial\'', () => {
                it('then all component\'s headers should contain \'Surveyor\'', () => {
                    var division = _.find(divisionCodes, (division) => division.id === 'commmercialId');
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
                    var division = _.find(divisionCodes, (division) => division.id === 'residentialId');
                    controller.labelTranslationKey = division.code;
                    scope.$apply();

                    var mainHeader: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentNegotiatorMainHeader);
                    var subHeaders: ng.IAugmentedJQuery = element.find(pageObjectSelector.componentNegotiatorSubHeaders);

                    expect(mainHeader.text()).toContain(division.code);
                    _.each(subHeaders, (header) => expect(header.innerText).toContain(division.code));
                });
            });

            describe('when secondary negotiator edit mode is changed', () => {
                it('to true then search component is displayed and an \'Add\' button is hidden', () => {
                    controller.isSecondaryNegotiatorsInEditMode = true;
                    scope.$apply();

                    assertValidator.assertElementHasHideClass(false, pageObjectSelector.secondarySearch);
                    expect(element.find(pageObjectSelector.addSecondaryBtn).length).toBe(0);
                });

                it('to false then search component is hidden and an \'Add\' button is displayed', () => {
                    controller.isSecondaryNegotiatorsInEditMode = false;
                    scope.$apply();

                    assertValidator.assertElementHasHideClass(true, pageObjectSelector.secondarySearch);
                    expect(element.find(pageObjectSelector.addSecondaryBtn).length).toBe(1);
                });
            });

            describe('when lead negotiator edit mode is changed', () => {
                it('to true then search component is displayed and an \'Edit\' button is hidden', () => {
                    controller.isLeadNegotiatorInEditMode = true;
                    scope.$apply();

                    assertValidator.assertElementHasHideClass(false, pageObjectSelector.leadSearch);
                    assertValidator.assertElementHasHideClass(true, pageObjectSelector.editLeadNegotiatorBtnWrapper);
                });

                it('to false then lead negotiator search component is hidden and an \'Edit\' button is visible', () => {
                    controller.isLeadNegotiatorInEditMode = false;
                    scope.$apply();

                    assertValidator.assertElementHasHideClass(true, pageObjectSelector.leadSearch);
                    assertValidator.assertElementHasHideClass(false, pageObjectSelector.editLeadNegotiatorBtnWrapper);
                });
            });

            describe('when new negotiator is selected', () => {
                it('as lead then current lead negotiator should be updated', () => {
                    var leadSearch = element.find(pageObjectSelector.leadSearch);
                    var searchController = leadSearch.controller('search');
                    var newLeadNegotiatorUser = <Dto.IDepartmentUser>TestHelpers.UserGenerator.generateDto();

                    searchController.select(newLeadNegotiatorUser);

                    expect(angular.equals(controller.leadNegotiator.user, newLeadNegotiatorUser)).toBeTruthy();
                });

                it('as secondary then secondary negotiator should be added', () => {
                    var secodnarySearch = element.find(pageObjectSelector.secondarySearch);
                    var searchController = secodnarySearch.controller('search');
                    var newSecondarynegotiatorUser = <Dto.IDepartmentUser>TestHelpers.UserGenerator.generateDto();

                    searchController.select(newSecondarynegotiatorUser);

                    expect(_.find(controller.secondaryNegotiators, (negotiator) => negotiator.user.id === newSecondarynegotiatorUser.id)).toBeDefined();
                });
            });

            describe('when one of secondary negotiator is deleted', () => {
                it('then list of secondary negotiators should contain one element less', () => {
                    // arrange
                    var originalSecondaryNegotiatorsCount: number = secondaryNegotiators.length;
                    var expectedElementCount = --originalSecondaryNegotiatorsCount;

                    // act
                    controller.deleteSecondaryNegotiator(secondaryNegotiators[0]);
                    scope.$apply();

                    var result = element.find(pageObjectSelector.secondaryNegotiatorItems);

                    // arrange
                    expect(scope.secondaryNegotiator.length).toEqual(expectedElementCount);
                    expect(result.length).toEqual(expectedElementCount);
                });
            });

            // RequiredValidator for call date
            type TestCaseForValidator = [string, boolean];
            runDescribe('when lead negotiator call date is filled and ')
                .data<TestCaseForValidator>([
                    [datesToTest.inTheFuture, true],
                    [null, false],
                    ['', false]])
                .dataIt((data: TestCaseForValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelector.leadCallDate);
                });

            runDescribe('when secondary negotiator call date is filled and ')
                .data<TestCaseForValidator>([
                    [datesToTest.inTheFuture, true],
                    [null, true],
                    ['', true]])
                .dataIt((data: TestCaseForValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelector.secondaryCallDatePrefix + secondaryNegotiators[0].userId);
                });

            runDescribe('when lead negotiator call date is filled and ')
                .data<TestCaseForValidator>([
                    [datesToTest.inTheFuture, true],
                    [datesToTest.today, true],
                    [datesToTest.inThePast, false],
                    [datesToTest.longAgo, false]])
                .dataIt((data: TestCaseForValidator) =>
                    `value is "${data[0]}" then date greater then message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForValidator) => {
                    // arrange / act / assert
                    assertValidator.assertDateGreaterThenValidator(data[0], data[1], pageObjectSelector.leadCallDate);
                });


        });
    });
}