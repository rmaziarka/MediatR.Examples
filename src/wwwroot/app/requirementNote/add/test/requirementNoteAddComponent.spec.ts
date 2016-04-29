/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementNoteAddController = RequirementNote.RequirementNoteAddController;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;

    describe('Given requirement note add component is displayed', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: RequirementNoteAddController;

        var pageObjectSelectors = {
            noteAddDescription: 'textarea#note-description',
            noteAddButton: 'button#note-add-button'
        };

        beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;
                compile = $compile;
                scope = $rootScope.$new();

                scope['requirement'] = TestHelpers.RequirementGenerator.generate({ requirementNotes: [] });
                element = compile('<requirement-note-add requirement="requirement"></requirement-note-add>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }
        ));

        // RequiredValidator
        // Specify exact type on test cases data for strong typing within test body (helps with intelisense).
        type TestCaseForRequiredValidator = [string, boolean];
        runDescribe('when filling note description')
            .data<TestCaseForRequiredValidator>([
                ['abc', true],
                ['1', true],
                ['', false],
                [null, false]])
            .dataIt((data: TestCaseForRequiredValidator) =>
                `and value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForRequiredValidator) => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.noteAddDescription);
            });

        // MaxLengthValidator
        // Specify exact type on test cases data for strong typing within test body (helps with intelisense).
        type TestCaseForMaxLengthValidator = [number, boolean];
        runDescribe('when filling note description')
            .data<TestCaseForMaxLengthValidator>([
                [1, true],
                [4000, true],
                [4001, false]])
            .dataIt((data: TestCaseForMaxLengthValidator) =>
                `and value has length "${data[0]}" then validation message should ${data[1] ? 'not' : ''} be displayed`)
            .run((data: TestCaseForMaxLengthValidator) => {
                // arrange / act / assert
                assertValidator.assertMaxLengthValidator(data[0], data[1], pageObjectSelectors.noteAddDescription);
            });

        describe('when "Save button" is clicked ', () => {
            var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({ requirementNotes: [] });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['requirement'] = requirementMock;
                element = compile('<requirement-note-add requirement="requirement"></requirement-note-add>')(scope);
                scope.$apply();

                controller = element.controller('requirementNoteAdd');
            }
            ));

            it('then save method is called', () => {
                // arrange
                spyOn(controller, 'saveNote');
                controller.requirementNote.description = "Test description";
                scope.$apply();

                // act
                var button = element.find(pageObjectSelectors.noteAddButton);
                button.click();

                //assert
                expect(controller.saveNote).toHaveBeenCalled();
            });

            it('then proper data should be send to API', () => {
                // arrange
                controller.requirementNote.description = "Test description";
                scope.$apply();

                var urlRegex = new RegExp('api/requirements/' + requirementMock.id),
                    requestData: Business.CreateRequirementNoteResource;

                $http.expectPOST(urlRegex, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                }).respond(201, new Business.RequirementNote());

                // act
                var button = element.find(pageObjectSelectors.noteAddButton);
                button.click();
                $http.flush();

                //assert
                expect(requestData.requirementId).toEqual(requirementMock.id);
                expect(requestData.description).toEqual("Test description");
            });

            it('then new note should be added to requirement notes list', () => {
                // arrange
                controller.requirementNote.description = "Test description";
                scope.$apply();

                var urlRegex = new RegExp('api/requirements/' + requirementMock.id),
                    expectedResponse = new Business.RequirementNote();

                expectedResponse.id = 'idFromServer';
                $http.whenPOST(urlRegex).respond(201, expectedResponse);

                 // act
                element.find(pageObjectSelectors.noteAddButton).click();
                $http.flush();

                //assert
                expect(requirementMock.requirementNotes.filter((item) => { return item.id === expectedResponse.id }).length).toBe(1);
            });

            it('then description field is cleared', () => {
                // arrange
                var urlRegex = new RegExp('api/requirements/' + requirementMock.id),
                    expectedResponse = new Business.RequirementNote();

                expectedResponse.id = 'idFromServer';
                $http.whenPOST(urlRegex).respond(201, expectedResponse);

                // act
                var descriptionElement = element.find(pageObjectSelectors.noteAddDescription);
                descriptionElement.val('abc').trigger('input').trigger('change').trigger('blur');
                scope.$apply();

                element.find(pageObjectSelectors.noteAddButton).click();
                $http.flush();

                //assert
                expect(descriptionElement.text()).toBe('');
            });
        });
    });
}