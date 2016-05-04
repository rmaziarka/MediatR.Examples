/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingDetailsController = Antares.Component.ViewingDetailsController;
    import Dto = Common.Models.Dto;

    import runDescribe = TestHelpers.runDescribe;

    describe('Given viewing is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            $compileService: ng.ICompileService;

        var pageObjectSelectors = {
            viewingDateSelector: '[name=viewingStartDate]',
            startTimeSelector: '[name=startTime]',        
            startTimeInputSelector: '[name=startTime] input',        
            endTimeSelector: '[name=endTime]',
            endTimeInputSelector: '[name=endTime] input',
            invitationTextSelector: '#invitation-text',
            postViewingComment: '#post-viewing-comment',
            atendeesList: 'list-not-empty',
            requiredValidatorSelector: '[name="requiredValidationError"]',
            timeValidatorSelector: '[name="timeValidationError"]',
            listNotEmptyValidatorSelector: '[name="listNotEmptyValidationError"]',
            timeLowerValidatorSelector: '[name = "timeLowerThanValidationError"]',
            timeGreaterValidatorSelector: '[name = "timeGreaterThanValidationError"]'
        };
        
        var pageObject = {
            inputValidCss: 'ng-valid'
        };

        var controller: ViewingDetailsController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            $compileService = $compile;
            scope = $rootScope.$new();
            scope["attendees"] = [];
            element = $compile('<viewing-details attendees="vm.attendees"></viewing-details>')(scope);
            scope.$apply();

            controller = element.controller('viewingDetails');
            controller.startTime = "10:00";
            controller.endTime = "11:00";
            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        describe('when', () => {          
            type TestCaseForRequiredValidator = [string, boolean];
            // RequiredValidator for viewing date
            runDescribe('viewing date ')
                .data<TestCaseForRequiredValidator>([
                    ['', false],
                    ['21-12-1984', true],
                    ['invalid date', false]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
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

            xit('end time is lesser than start time then validation message should be displayed', () => {
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
                expect(endTimewrapper.parent().find(pageObjectSelectors.timeLowerValidatorSelector).length).toBe(1);
            });

            it('no atendees are selected then validation message should be displayed', () => {
                // arrange
                var list = element.find(pageObjectSelectors.atendeesList);
                // act
                var form = element.find('form').triggerHandler('submit');
                // assert
                expect(list.hasClass(pageObject.inputValidCss)).toBe(false);
                expect(element.find(pageObjectSelectors.listNotEmptyValidatorSelector).length).toBe(1);
            });

            runDescribe('start time')
                .data<TestCaseForRequiredValidator>([                    
                    ['11:00', true],
                    ['11:70', false],
                    ['26:00', false,],
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
    });
}