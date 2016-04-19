/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementNoteAddController = RequirementNote.RequirementNoteAddController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

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

        describe('when filling note description', () => {
            it('and value is missing then required message should be displayed', () => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.noteAddDescription);
            });

            it('and value is not missing then required message should not be displayed', () => {
                // arrange / act / assert
                assertValidator.assertRequiredValidator('2', true, pageObjectSelectors.noteAddDescription);
            });

            it('and value is to long then validation message should be displayed', () => {
                // arrange / act / assert
                var maxLength = 4000;
                assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.noteAddDescription);
            });

            it('and value is not to long then validation message should be displayed', () => {
                // arrange / act / assert
                var maxLength = 4000;
                assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.noteAddDescription);
            });
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