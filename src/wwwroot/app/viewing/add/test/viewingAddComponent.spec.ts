/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingAddController = Component.ViewingAddController;
    import Business = Common.Models.Business;
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseForRequiredValidator = [string, boolean];

    declare var moment: any;

    describe('Given viewing', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: ViewingAddController,
            compile: ng.ICompileService;

        var pageObjectSelectors = {
            viewingDateSelector: '[name=viewingStartDate]',
            startTimeSelector: '[name=startTime]',        
            startTimeInputSelector: '[name=startTime] input',        
            endTimeSelector: '[name=endTime]',
            endTimeInputSelector: '[name=endTime] input',
            invitationTextSelector: '#invitation-text',
            postViewingComment: '#post-viewing-comment',
            atendeesListValidator: 'list-not-empty',
            requiredValidatorSelector: '[name="requiredValidationError"]',
            timeValidatorSelector: '[name="timeValidationError"]',
            listNotEmptyValidatorSelector: '[name="listNotEmptyValidationError"]',
            timeLowerValidatorSelector: '[name = "timeLowerThanValidationError"]',
            timeGreaterValidatorSelector: '[name = "timeGreaterThanValidationError"]',
            attendesElementsSelector: '#attendees-list input:checkbox',
            viewingActivitySelector: '#viewing-activity'
        };
        
        var pageObject = {
            inputValidCss: 'ng-valid'
        };

        var activityMock = TestHelpers.ActivityQueryResultGenerator.generate();

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
            contacts: [
                { id: '1', firstName: 'John', surname: 'Doe', title: 'Mr.' },
                { id: '2', firstName: 'Jane', surname: 'Doe', title: 'Mrs.' }
            ]
        });

        describe('when component is in add mode', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                scope = $rootScope.$new();
                scope["requirement"] = requirementMock;
                scope["attendees"] = requirementMock.contacts;

                element = $compile('<viewing-add mode="add" requirement="requirement" attendees="attendees"></viewing-add>')(scope);
                scope.$apply();

                controller = element.controller('viewingAdd');
                controller.startTime = "10:00";
                controller.endTime = "11:00";

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            // RequiredValidator for viewing date
            runDescribe('viewing date ')
                .data<TestCaseForRequiredValidator>([
                    ['', false],
                    ['21-12-1984', true]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required validation message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.viewingDateSelector);
                });

            it('start time is empty then validation message should be displayed', () => {
                // arrange
                var wrapper = element.find(pageObjectSelectors.startTimeSelector);
                var input = element.find(pageObjectSelectors.startTimeInputSelector);   
                // act            
                controller.startTime = null;
                input.val(null).trigger('input').trigger('change').trigger('blur');                   
                // assert
                expect(wrapper.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(wrapper.parent().find(pageObjectSelectors.requiredValidatorSelector).length).toBe(1);
            });

            it('end time is empty then validation message should be displayed', () => {
                // arrange
                var wrapper = element.find(pageObjectSelectors.endTimeSelector);
                var input = element.find(pageObjectSelectors.endTimeInputSelector);
                // act                
                controller.endTime = null;
                input.val(null).trigger('input').trigger('change').trigger('blur');         
                //assert
                expect(wrapper.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(wrapper.parent().find(pageObjectSelectors.requiredValidatorSelector).length).toBe(1);
            });

            it('end time is lesser than start time then validation message should be displayed', () => {
                // arrange
                var startTimewrapper = element.find(pageObjectSelectors.startTimeSelector);
                var endTimewrapper = element.find(pageObjectSelectors.endTimeSelector);
                var startTimeInput = element.find(pageObjectSelectors.startTimeInputSelector);
                var endTimeinput = element.find(pageObjectSelectors.endTimeInputSelector);
                // act     
                startTimeInput.val("11:00").trigger('input').trigger('change').trigger('blur');
                endTimeinput.val("10:00").trigger('input').trigger('change').trigger('blur');         
                //assert
                expect(startTimewrapper.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(endTimewrapper.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(startTimewrapper.parent().find(pageObjectSelectors.timeLowerValidatorSelector).length).toBe(1);
                expect(endTimewrapper.parent().find(pageObjectSelectors.timeGreaterValidatorSelector).length).toBe(1);
            });

            it('no atendees are selected then validation message should be displayed', () => {
                // arrange
                var listValidator = element.find(pageObjectSelectors.atendeesListValidator);
                // act
                var form = element.find('form').triggerHandler('submit');
                // assert
                expect(listValidator.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(element.find(pageObjectSelectors.listNotEmptyValidatorSelector).length).toBe(1);
            });

            it('atendees are selected and unselected then validation message should be displayed', () => {
                // arrange
                var list = element.find(pageObjectSelectors.atendeesListValidator);
                // act
                element.find(pageObjectSelectors.attendesElementsSelector).first().click();
                element.find(pageObjectSelectors.attendesElementsSelector).first().click();
                // assert
                expect(list.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(element.find(pageObjectSelectors.listNotEmptyValidatorSelector).length).toBe(1);
            });

            it('selected activity is displayed', () => {
                // arrange
                var activityElement = element.find(pageObjectSelectors.viewingActivitySelector); 
                // act
                controller.setActivity(activityMock);
                scope.$apply();               
                // assert
                expect(activityElement.text()).toContain(activityMock.propertyName);
            });

            runDescribe('start time')
                .data<TestCaseForRequiredValidator>([
                    ['11:00', true],
                    ['11:70', false],
                    ['26:00', false],
                    ['invalid time', false]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange     
                    var wrapper = element.find(pageObjectSelectors.startTimeSelector);
                    var input = element.find(pageObjectSelectors.startTimeInputSelector);
                    controller.startTime = data[0];
                    // act
                    input.val(data[0]).trigger('input').trigger('change').trigger('blur');
                    scope.$apply();
                    //assert
                    expect(wrapper.hasClass(pageObject.inputValidCss)).toBe(data[1]);
                    //expect(wrapper.parent().find(pageObjectSelectors.timeValidatorSelector).length).toBe(1);
                });

            runDescribe('end time')
                .data<TestCaseForRequiredValidator>([
                    ['11:00', true],
                    ['11:70', false],
                    ['26:00', false],
                    ['invalid time', false]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange
                    var wrapper = element.find(pageObjectSelectors.endTimeSelector);
                    var input = element.find(pageObjectSelectors.endTimeInputSelector);
                    controller.endTime = data[0];
                    // act
                    input.val(data[0]).trigger('input').trigger('change').trigger('blur');
                    scope.$apply();
                    // assert
                    expect(wrapper.hasClass(pageObject.inputValidCss)).toBe(data[1]);                   
                    //expect(wrapper.parent().find(pageObjectSelectors.timeValidatorSelector).length).toBe(1);                    
                });
            
            it('invitation text length is too long then validation message should be displayed', () => {
                var maxLength = 4000;
                assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.invitationTextSelector);
            });
        });

        describe('when component is in edit mode', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                scope = $rootScope.$new();
                compile = $compile;
                scope["requirement"] = requirementMock;
                scope["attendees"] = requirementMock.contacts;

                element = compile('<viewing-add mode="edit" requirement="requirement" attendees="attendees"></viewing-add>')(scope);
                scope.$apply();
                controller = element.controller('viewingAdd');
                controller.startTime = "10:00";
                controller.endTime = "11:00";
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('all data is loaded', () => {
                // arange
                var viewingMock = TestHelpers.ViewingGenerator.generate();
                var formattedDate = moment(viewingMock.startDate).format('DD-MM-YYYY');
                var formattedStartTime = moment(viewingMock.startDate).format('HH:mm');
                var formattedEndTime = moment(viewingMock.endDate).format('HH:mm');
                
                // act                
                controller.setViewing(viewingMock);
                scope.$apply();

                // assert                
                expect(element.find(pageObjectSelectors.attendesElementsSelector).length).toBe(requirementMock.contacts.length);
                expect(element.find(pageObjectSelectors.viewingDateSelector).val()).toBe(formattedDate);
                expect(element.find(pageObjectSelectors.startTimeInputSelector).val()).toBe(formattedStartTime);
                expect(element.find(pageObjectSelectors.endTimeInputSelector).val()).toBe(formattedEndTime);
                expect(element.find(pageObjectSelectors.viewingActivitySelector).text()).toContain(viewingMock.activity.property.address.propertyName);
                expect(element.find(pageObjectSelectors.invitationTextSelector).val()).toBe(viewingMock.invitationText);
                expect(element.find(pageObjectSelectors.postViewingComment).val()).toBe(viewingMock.postViewingComment);
            });
        });
    });
}